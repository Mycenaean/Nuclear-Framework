// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Auth;
using Nuclear.Channels.Auth.Identity;
using Nuclear.Channels.Base;
using Nuclear.Channels.Contracts;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Enums;
using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Channels.Hosting.Deserializers;
using Nuclear.Channels.Hosting.Exceptions;
using Nuclear.Channels.Interfaces;
using Nuclear.Data.Logging.Enums;
using Nuclear.Data.Logging.Services;
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
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Hosting
{
    [Export(typeof(IChannelActivator), Lifetime = ExportLifetime.Singleton)]
    public class ChannelActivator : IChannelActivator, IExecutor
    {
        private IServiceLocator _services;
        private IChannelLocator _channelLocator;
        private IChannelMethodDescriptor _channelMethodDescriptor;
        private IChannelMethodRequestActivator _requestActivator;
        private IChannelMessageService _msgService;
        private IChannelMethodContextProvider _contextProvider;
        private string BaseURL = null;
        private Func<string, string, bool> _basicAuthenticationMethod;
        private Func<string, bool> _tokenAuthenticationMethod;
        private static List<Cookie> _sessionKeys;
        private string _rootPath;

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

        public void Execute(AppDomain domain, IServiceLocator _Services, string baseURL = null)
        {
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
                BaseURL = baseURL;

            _channelLocator = _services.Get<IChannelLocator>();
            _channelMethodDescriptor = _services.Get<IChannelMethodDescriptor>();
            _requestActivator = _services.Get<IChannelMethodRequestActivator>();
            _msgService = _services.Get<IChannelMessageService>();
            _contextProvider = _services.Get<IChannelMethodContextProvider>();

            Debug.Assert(_channelLocator != null);
            Debug.Assert(_channelMethodDescriptor != null);
            Debug.Assert(_requestActivator != null);
            Debug.Assert(_msgService != null);
            Debug.Assert(_contextProvider != null);

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
            ChannelEndpoint endpoint = new ChannelEndpoint();
            ChannelAttribute channelAttr = channel.GetCustomAttribute<ChannelAttribute>();
            if (!String.IsNullOrEmpty(channelAttr.Name))
                endpoint.URL = "/channels/" + channelAttr.Name + "/" + method.Name + "/";
            else
                endpoint.URL = "/channels/" + channel.Name + "/" + method.Name + "/";

            endpoint.Name = channel.Name + "." + method.Name;

            ChannelMethodAttribute ChannelMethod = method.GetCustomAttribute<ChannelMethodAttribute>();
            AuthorizeChannelAttribute authAttr = channel.GetCustomAttribute<AuthorizeChannelAttribute>();
            ChannelAuthenticationSchemes ChannelSchema = ChannelMethod.Schema;
            ChannelHttpMethod HttpMethod = ChannelMethod.HttpMethod;
            HttpListener httpChannel = new HttpListener();

            string methodURL = string.Empty;
            if (BaseURL == null)
                methodURL = "http://localhost:4200" + endpoint.URL;
            else
                methodURL = BaseURL + endpoint.URL;

            bool httpAuthRequired = false;

            //Start hosting
            httpChannel.Prefixes.Add(methodURL);
            if (authAttr != null)
            {
                if (authAttr.Schema != ChannelAuthenticationSchemes.Anonymous)
                {
                    httpAuthRequired = true;
                }
            }
            //ChannelMethod can override ChannelAttribute Authentication Schemes
            if (ChannelSchema != ChannelAuthenticationSchemes.Anonymous)
            {
                httpAuthRequired = true;
            }
            else
            {
                if (authAttr != null)
                    ChannelSchema = authAttr.Schema;
            }

            //Keep the ChannelMethod open for new requests
            while (true)
            {
                try
                {
                    httpChannel.Start();
                }
                catch (HttpListenerException hle)
                {
                    Console.WriteLine($"System.Net.HttpListenerException encountered on {methodURL} with reason :{hle.Message}");
                    return;
                }

                Console.WriteLine($"Listening on {methodURL}");
                HttpListenerContext context = httpChannel.GetContext();
                HttpListenerRequest request = context.Request;

                LogChannel.Write(LogSeverity.Info, $"Request coming to {endpoint.Name}");
                LogChannel.Write(LogSeverity.Info, $"HttpMethod:{request.HttpMethod}");
                LogChannel.Write(LogSeverity.Info, $"IsAuthenticated:{request.IsAuthenticated}");

                HttpListenerResponse response = context.Response;
                bool validCookie = false;
                bool authenticated = false;
                if (channelAttr.EnableSessions)
                    validCookie = ValidSession(request);
                if (httpAuthRequired && !validCookie)
                {
                    authenticated = CheckAuthentication(context, ChannelSchema, response);
                    if (!authenticated)
                        goto AuthenticationFailed;
                    else
                    {
                        if (channelAttr.EnableSessions)
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

                //Check if the Http Method is correct
                if (HttpMethod.ToString() != request.HttpMethod && HttpMethod != ChannelHttpMethod.Unknown)
                {
                    _msgService.WrongHttpMethod(response, HttpMethod);
                    LogChannel.Write(LogSeverity.Error, "Wrong HttpMethod... Closing request");
                }


                Dictionary<string, Type> methodDescription = _channelMethodDescriptor.GetMethodDescription(method);
                List<object> channelRequestBody = null;
                ChannelMethodDeserializerFactory dsrFactory = null;

                if (request.HttpMethod == "GET" && request.QueryString.AllKeys.Length > 0)
                {
                    dsrFactory = new ChannelMethodDeserializerFactory(request.QueryString);
                    channelRequestBody = dsrFactory.DeserializeFromQueryParameters(methodDescription);
                    InitChannelMethodContext(endpoint, request, response, authenticated, HttpMethod, channelRequestBody);
                    _requestActivator.GetActivateWithParameters(channel, method, channelRequestBody, response);
                    _contextProvider.DestroyChannelMethodContext(endpoint);
                }
                else if (request.HttpMethod == "GET" && request.QueryString.AllKeys.Length == 0)
                {
                    if (methodDescription.Count > 0)
                    {
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _msgService.ExceptionHandler(writer, new TargetParameterCountException(), response);
                        writer.Close();
                        goto AuthenticationFailed;
                    }

                    InitChannelMethodContext(endpoint, request, response, authenticated, HttpMethod, channelRequestBody);
                    _requestActivator.GetActivateWithoutParameters(channel, method, response);
                    _contextProvider.DestroyChannelMethodContext(endpoint);
                }

                //Enter only if Request Body is supplied with POST Method
                if (request.HasEntityBody == true && request.HttpMethod == "POST")
                {
                    StreamWriter writer = new StreamWriter(response.OutputStream);
                    try
                    {
                        dsrFactory = new ChannelMethodDeserializerFactory(request.InputStream);
                        channelRequestBody = dsrFactory.DeserializeFromBody(methodDescription, request.ContentType);
                        InitChannelMethodContext(endpoint, request, response, authenticated, HttpMethod, channelRequestBody);
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
                    finally
                    {
                        _contextProvider.DestroyChannelMethodContext(endpoint);
                        writer.Flush();
                        writer.Close();
                    }
                }

            AuthenticationFailed:
                LogChannel.Write(LogSeverity.Debug, "Request finished...");
                LogChannel.Write(LogSeverity.Debug, "Closing the response");
                response.Close();
            }

        }

        private void OneTimeSetup()
        {
            _rootPath = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName;
            DirectoryInfo logsDirectory = new DirectoryInfo(Path.Combine(_rootPath, "Logs"));
            if (!Directory.Exists(logsDirectory.FullName))
                logsDirectory.Create();

            _sessionKeys = new List<Cookie>();
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

        private bool CheckAuthentication(HttpListenerContext context, ChannelAuthenticationSchemes ChannelSchema, HttpListenerResponse response)
        {
            bool authenticated = false;
            HttpListenerIdentityService identityService = new HttpListenerIdentityService(_basicAuthenticationMethod, _tokenAuthenticationMethod);
            StreamWriter writer = new StreamWriter(response.OutputStream);
            try
            {
                bool knownUser = identityService.AuthenticatedAndAuthorized(context, ChannelSchema);
                if (!knownUser)
                    _msgService.FailedAuthenticationResponse(ChannelSchema, response);
                else
                    authenticated = true;

                return authenticated;
            }
            catch (ChannelCredentialsException cEx)
            {
                response.StatusCode = 401;
                _msgService.ExceptionHandler(writer, cEx, response);
                LogChannel.Write(LogSeverity.Error, cEx.Message);
                writer.Flush();
                writer.Close();
                return false;
            }
            catch (HttpListenerException hEx)
            {
                _msgService.ExceptionHandler(writer, hEx, response);
                LogChannel.Write(LogSeverity.Error, hEx.Message);
                writer.Flush();
                writer.Close();
                return false;
            }
            //This will close the stream on POST methods causing it to fail and if writer is only flushed then there is possibility of memory leaks (need inspection)
            //finally
            //{
            //    writer.Flush();
            //    writer.Close();
            //}
        }
    }
}
