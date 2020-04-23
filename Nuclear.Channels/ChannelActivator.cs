// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.Channels.Authentication.Identity;
using Nuclear.Channels.Base;
using Nuclear.Channels.Base.Contracts;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Base.Exceptions;
using Nuclear.Channels.Data.Deserializers;
using Nuclear.Channels.Data.Logging;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Heuristics;
using Nuclear.Channels.Heuristics.Contexts;
using Nuclear.Channels.InvokerServices.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels
{
    [Export(typeof(IChannelActivator), Lifetime = ExportLifetime.Singleton)]
    internal class ChannelActivator : IChannelActivator, IExecutor
    {
        private IServiceLocator _services;
        private IChannelLocator _channelLocator;
        private IChannelMethodDescriptor _channelMethodDescriptor;
        private IChannelMethodRequestActivator _requestActivator;
        private IChannelMessageService _msgService;
        private IChannelMethodContextProvider _contextProvider;
        private IChannelConfiguration _configuration;
        private IChannelAuthenticationService _authenticationService;
        private IChannelHeuristics _heuristics;
        private AuthenticationSettings _settings;
        private Func<string, string, bool> _basicAuthenticationMethod;
        private Func<string, bool> _tokenAuthenticationMethod;
        private static List<Cookie> _sessionKeys;
        private string _rootPath;
        private string _baseURL = null;

        [DebuggerStepThrough]
        public ChannelActivator()
        {

        }

        public void AuthenticationOptions(Func<string, string, bool> basicAuthMethod)
        {
            _basicAuthenticationMethod = basicAuthMethod ?? throw new ArgumentNullException("Authentication function must not be null");
        }

        public void AuthenticationOptions(Func<string, bool> tokenAuthMethod)
        {
            _tokenAuthenticationMethod = tokenAuthMethod ?? throw new ArgumentNullException("Authentication function must not be null");
        }

        private void OneTimeSetup()
        {
            _channelLocator = _services.Get<IChannelLocator>();
            _channelMethodDescriptor = _services.Get<IChannelMethodDescriptor>();
            _requestActivator = _services.Get<IChannelMethodRequestActivator>();
            _msgService = _services.Get<IChannelMessageService>();
            _contextProvider = _services.Get<IChannelMethodContextProvider>();
            _configuration = _services.Get<IChannelConfiguration>();
            _authenticationService = _services.Get<IChannelAuthenticationService>();
            _heuristics = _services.Get<IChannelHeuristics>();

            Debug.Assert(_channelLocator != null);
            Debug.Assert(_channelMethodDescriptor != null);
            Debug.Assert(_requestActivator != null);
            Debug.Assert(_msgService != null);
            Debug.Assert(_contextProvider != null);
            Debug.Assert(_configuration != null);
            Debug.Assert(_authenticationService != null);
            Debug.Assert(_heuristics != null);

            _rootPath = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName;
            DirectoryInfo logsDirectory = new DirectoryInfo(Path.Combine(_rootPath, "Logs"));
            if (!Directory.Exists(logsDirectory.FullName))
                logsDirectory.Create();

            _sessionKeys = new List<Cookie>();
        }

        public void Execute(AppDomain domain, IServiceLocator _Services, AuthenticationSettings settings, string baseURL = null)
        {
            _settings = settings;
            _services = _Services;
            Debug.Assert(HttpListener.IsSupported);
            Debug.Assert(_Services != null);

            OneTimeSetup();

            if (!HttpListener.IsSupported)
            {
                LogChannel.Write(LogSeverity.Fatal, "HttpListener not supported");
                LogChannel.Write(LogSeverity.Fatal, "Exiting ChannelActivator...");
                throw new HttpListenerNotSupportedException("HttpListener is not supported");
            }
            if (baseURL != null)
                _baseURL = baseURL;

            List<Type> channels = new List<Type>();
            channels = _channelLocator.RegisteredChannels(domain);

            //Initialization part
            foreach (Type channel in channels)
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                Task task = new Task(() => MethodExecute(channel, token), token);
                task.Start();
            }
        }

        public void MethodExecute(Type channel, CancellationToken cancellationToken)
        {
            MethodInfo[] methods = channel.GetMethods().Where(x => x.GetCustomAttribute<ChannelMethodAttribute>() != null).ToArray();

            foreach (MethodInfo method in methods)
            {
                cancellationToken.ThrowIfCancellationRequested();
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                Task task = new Task(() => StartListening(method, channel, token), token);
                task.Start();
            }
        }

        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "Used")]
        [SuppressMessage("Style", "IDE0049:Name can be simplified", Justification = "String is a class")]
        public void StartListening(MethodInfo method, Type channel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //Exception should be thrown to developer if EnableCache is on top of void Method
            CheckCacheValidity(method);

            HttpListener httpChannel = new HttpListener();

            ChannelConfigurationInfo channelConfig = _configuration.Configure(httpChannel, channel, method, _baseURL);

            //Keep the ChannelMethod open for new requests
            while (true)
            {
                try
                {
                    httpChannel.Start();
                }
                catch (HttpListenerException hle)
                {
                    Console.WriteLine($"System.Net.HttpListenerException encountered on {channelConfig.MethodUrl} with reason :{hle.Message}");
                    return;
                }

                Console.WriteLine($"Listening on {channelConfig.MethodUrl}");
                HttpListenerContext context = httpChannel.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                IChannelHeuristicContext heuristicsCtx = _services.Get<IChannelHeuristicContext>();

                LogChannel.Write(LogSeverity.Info, $"Request coming to {channelConfig.Endpoint.Name}");
                LogChannel.Write(LogSeverity.Info, $"HttpMethod:{request.HttpMethod}");

                //Even if method is cached check authenticaion first
                bool authFailed = AuthenticationFailedIfRequired(context, request, response, channelConfig, out bool authenticated);
                if (authFailed)
                    goto EndRequest;

                List<object> channelRequestBody = null;

                
                CacheExecutionResult cacheExecutionResult = ExecuteIfCached(channel, method, request, response, heuristicsCtx);
                if (cacheExecutionResult.Executed)
                {
                    heuristicsCtx.Clear();
                    goto EndRequest;
                }

                if (channelConfig.HttpMethod.ToString() != request.HttpMethod && channelConfig.HttpMethod != ChannelHttpMethod.Unknown)
                {
                    _msgService.WrongHttpMethod(response, channelConfig.HttpMethod);
                    LogChannel.Write(LogSeverity.Error, "Wrong HttpMethod... Closing request");
                    goto EndRequest;
                }

                ChannelMethodInfo methodDescription = _channelMethodDescriptor.GetMethodDescription(method);
                ChannelMethodDeserializerFactory dsrFactory = null;

                if (request.HttpMethod == "GET")
                {
                    TryInvokeGetRequest(channel, method, request, response, dsrFactory, methodDescription, channelConfig, channelRequestBody, authenticated,cacheExecutionResult);
                    goto EndRequest;
                }

                //Enter only if Request Body is supplied with POST Method
                if (request.HasEntityBody == true && request.HttpMethod == "POST")
                {
                    TryInvokePostRequest(channel, method, request, response, dsrFactory, channelRequestBody, methodDescription, channelConfig, authenticated, cacheExecutionResult);
                }

            EndRequest:
                LogChannel.Write(LogSeverity.Debug, "Request finished...");
                LogChannel.Write(LogSeverity.Debug, "Closing the response");
                response.Close();
            }

        }

        private void TryInvokePostRequest(Type channel,
            MethodInfo method,
            HttpListenerRequest request,
            HttpListenerResponse response,
            ChannelMethodDeserializerFactory dsrFactory,
            List<object> channelRequestBody,
            ChannelMethodInfo methodDescription,
            ChannelConfigurationInfo channelConfig,
            bool authenticated,
            CacheExecutionResult cacheExecutionResult)
        {
            StreamWriter writer = new StreamWriter(response.OutputStream);
            try
            {
                //Since request body will be processed in Heuristics if cache is enabled
                //InputStream is flushed and data is already stored in Data.Parameters  
                //property of CacheExecutionResult
                if (!cacheExecutionResult.DataProcessed)
                {
                    dsrFactory = new ChannelMethodDeserializerFactory(request.InputStream);
                    channelRequestBody = dsrFactory.DeserializeFromBody(methodDescription, request.ContentType);
                }
                else
                    channelRequestBody = cacheExecutionResult.Data.Parameters;

                InitChannelMethodContext(channelConfig.Endpoint, request, response, authenticated, channelConfig.HttpMethod, channelRequestBody);
                _requestActivator.PostActivate(channel, method, channelRequestBody, response);
            }
            catch (ChannelMethodContentTypeException cEx)
            {
                response.StatusCode = 400;
                _msgService.ExceptionHandler(writer, cEx, response);
                LogChannel.Write(LogSeverity.Error, cEx.Message);
            }
            catch (ChannelMethodParameterException pEx)
            {
                response.StatusCode = 400;
                _msgService.ExceptionHandler(writer, pEx, response);
                LogChannel.Write(LogSeverity.Error, pEx.Message);
            }
            catch (TargetParameterCountException tEx)
            {
                response.StatusCode = 400;
                _msgService.ExceptionHandler(writer, tEx, response);
                LogChannel.Write(LogSeverity.Error, tEx.Message);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                _msgService.ExceptionHandler(writer, ex, response);
                LogChannel.Write(LogSeverity.Fatal, ex.Message);
            }
            finally
            {
                _contextProvider.DestroyChannelMethodContext(channelConfig.Endpoint);
                writer.Flush();
                writer.Close();
            }

        }

        private bool TryInvokeGetRequest(Type channel,
            MethodInfo method,
            HttpListenerRequest request,
            HttpListenerResponse response,
            ChannelMethodDeserializerFactory dsrFactory,
            ChannelMethodInfo methodDescription,
            ChannelConfigurationInfo channelConfig,
            List<object> channelRequestBody,
            bool authenticated,
            CacheExecutionResult cacheExecutionResult)
        {
            try
            {
                if (request.QueryString.AllKeys.Length > 0)
                {
                    SetupAndInvokeGetRequest(channel, method, dsrFactory, request, response, methodDescription, channelConfig, channelRequestBody, authenticated, true, cacheExecutionResult);
                }
                else if (request.QueryString.AllKeys.Length == 0)
                {
                    if (methodDescription.Parameters.Count > 0)
                    {
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _msgService.ExceptionHandler(writer, new TargetParameterCountException(), response);
                        writer.Close();
                        return false;
                    }

                    SetupAndInvokeGetRequest(channel, method, dsrFactory, request, response, methodDescription, channelConfig, channelRequestBody, authenticated, false, cacheExecutionResult);
                }
                return true;
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(response.OutputStream))
                {
                    _msgService.ExceptionHandler(writer, ex, response);
                    LogChannel.Write(LogSeverity.Fatal, ex.Message);
                }

                return true;
            }

        }

        private void SetupAndInvokeGetRequest(Type channel,
            MethodInfo method,
            ChannelMethodDeserializerFactory dsrFactory,
            HttpListenerRequest request,
            HttpListenerResponse response,
            ChannelMethodInfo methodDescription,
            ChannelConfigurationInfo channelConfig,
            List<object> channelRequestBody,
            bool authenticated,
            bool hasParams,
            CacheExecutionResult cacheExecutionResult)
        {
            //Context should be initialized before invoking the method because ChannelBase relies on Context
            InitChannelMethodContext(channelConfig.Endpoint, request, response, authenticated, channelConfig.HttpMethod, channelRequestBody);
            if (hasParams)
            {
                //Since request body will be processed in Heuristics if cache is enabled
                //Data is already stored in Data.Parameters property of CacheExecutionResult 
                if (!cacheExecutionResult.DataProcessed)
                {
                    dsrFactory = new ChannelMethodDeserializerFactory(request.QueryString);
                    channelRequestBody = dsrFactory.DeserializeFromQueryParameters(methodDescription);
                }
                else
                    channelRequestBody = cacheExecutionResult.Data.Parameters;

                _requestActivator.GetActivateWithParameters(channel, method, channelRequestBody, response);
            }
            else
                _requestActivator.GetActivateWithoutParameters(channel, method, response);

            _contextProvider.DestroyChannelMethodContext(channelConfig.Endpoint);
        }



        private bool ValidSession(HttpListenerRequest request)
        {
            Cookie sessionCookie = request.Cookies["channelAuthCookie"];
            if (sessionCookie == null)
                return false;
            bool validSessionKey = _sessionKeys.Any(x => x.Equals(sessionCookie));
            if (validSessionKey && !sessionCookie.Expired)
                return true;
            else if (validSessionKey && sessionCookie.Expired)
            {
                _sessionKeys.Remove(sessionCookie);
                return false;
            }
            else
                return false;

        }

        private void InitChannelMethodContext(IChannelEndpoint endpoint, HttpListenerRequest request, HttpListenerResponse response, bool isAuthenticated, ChannelHttpMethod method, List<object> channelRequestBody)
        {
            ChannelMethodContext methodContext = new ChannelMethodContext(request, response, method, channelRequestBody, isAuthenticated);
            _contextProvider.SetChannelMethodContext(endpoint, methodContext);
        }

        private bool IsCached(MethodInfo methodInfo)
        {
            EnableCacheAttribute cache = methodInfo.GetCustomAttribute<EnableCacheAttribute>();
            return cache != null;
        }

        private void CheckCacheValidity(MethodInfo methodInfo)
        {
            EnableCacheAttribute cache = methodInfo.GetCustomAttribute<EnableCacheAttribute>();
            if (methodInfo.ReturnType == typeof(void) && cache != null)
                throw new InvalidChannelMethodTargetException("EnableCache can not be applied to a method with return type void");
        }

        private CacheExecutionResult ExecuteIfCached(Type channel, MethodInfo method, HttpListenerRequest request, HttpListenerResponse response, IChannelHeuristicContext heurContext)
        {
            CacheExecutionResult result = new CacheExecutionResult();
            result.Executed = false;
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = IsCached(method);
            }
            catch (InvalidChannelMethodTargetException ex)
            {
                StreamWriter writer = new StreamWriter(response.OutputStream);
                _msgService.ExceptionHandler(writer, ex, response);
                writer.Close();
                result.Executed = true;
            }

            if (isCacheEnabled)
            {
                HeuristicsInfo hInfo = new HeuristicsInfo();
                bool isCached = _heuristics.IsMethodCached(channel, method, out hInfo);
                if (isCached)
                {
                    ChannelMethodHeuristicOptions hOptions = new ChannelMethodHeuristicOptions
                    {
                        Channel = channel,
                        ChannelMethod = method,
                        Request = request,
                        Response = response
                    };
                    return _heuristics.Execute(hOptions, hInfo);
                }
                else
                {
                    heurContext.ExpectsAdding = true;
                    heurContext.Channel = channel;
                    heurContext.MethodInfo = method;
                    result.Executed = false;
                    result.DataProcessed = false;
                }
            }

            return result;
        }

        private bool AuthenticationFailedIfRequired(HttpListenerContext context, HttpListenerRequest request, HttpListenerResponse response, ChannelConfigurationInfo channelConfig, out bool authenticated)
        {
            bool failed = false;
            bool validCookie = false;
            authenticated = false;
            bool authorized = false;
            if (channelConfig.ChannelAttribute.EnableSessions)
                validCookie = ValidSession(request);
            if (channelConfig.AuthenticationRequired && !validCookie)
            {
                try
                {
                    ChannelAuthenticationContext authContext = new ChannelAuthenticationContext
                    {
                        Context = context,
                        Scheme = channelConfig.AuthScheme,
                        BasicAuthenticationDelegate = _basicAuthenticationMethod,
                        TokenAuthenticationDelegate = _tokenAuthenticationMethod,
                        AuthenticationSettings = _settings

                    };

                    KeyValuePair<bool, object> authenticationResult = _authenticationService.CheckAuthenticationAndGetResponseObject(authContext);
                    if (authenticationResult.Key == true)
                        authenticated = true;
                    else
                    {
                        _msgService.FailedAuthenticationResponse(channelConfig.AuthScheme, response);
                        failed = true;
                    }
                    LogChannel.Write(LogSeverity.Info, "User Authenticated");
                    string claimName = channelConfig.AuthorizeAttribute.ClaimName;
                    string claimValue = channelConfig.AuthorizeAttribute.ClaimValue;
                    if (!String.IsNullOrEmpty(claimName) && !String.IsNullOrEmpty(claimValue))
                    {
                        if (authenticationResult.Value.GetType() == typeof(ClaimsPrincipal))
                            authorized = _authenticationService.Authorized(claimName, claimValue, (ClaimsPrincipal)authenticationResult.Value);
                        else
                            authorized = _authenticationService.Authorized(claimName, claimValue, (Claim[])authenticationResult.Value);

                        if (!authorized)
                        {
                            _msgService.FailedAuthorizationResponse(response);
                            LogChannel.Write(LogSeverity.Error, "Failed authorization");
                            failed = true;
                        }
                        else
                            LogChannel.Write(LogSeverity.Info, "User Authorized");
                    }
                }
                catch (Exception ex)
                {
                    using (StreamWriter writer = new StreamWriter(response.OutputStream))
                    {
                        _msgService.ExceptionHandler(writer, ex, response);
                        LogChannel.Write(LogSeverity.Error, "Authentication Failed");
                        failed = true;
                    }
                }
                if (!authenticated)
                    failed = true;
                else
                {
                    if (channelConfig.ChannelAttribute.EnableSessions)
                    {
                        string sessionKey = Guid.NewGuid().ToString();
                        Cookie sessionCookie = new Cookie()
                        {
                            Expires = DateTime.Now.AddMinutes(30),
                            Name = "channelAuthCookie",
                            Secure = true,
                            Value = sessionKey
                        };
                        response.SetCookie(sessionCookie);
                        _sessionKeys.Add(sessionCookie);
                    }
                }
            }

            return failed;
        }
    }
}
