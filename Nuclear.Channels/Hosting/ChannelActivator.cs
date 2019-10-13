using Newtonsoft.Json;
using Nuclear.Channels.Auth;
using Nuclear.Channels.Contracts;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Enums;
using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Channels.Hosting.Exceptions;
using Nuclear.Channels.Interfaces;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Messaging.Services.ChannelMessage;
using Nuclear.Data.Logging.Enums;
using Nuclear.Data.Logging.Services;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Formatting = Newtonsoft.Json.Formatting;

namespace Nuclear.Channels.Hosting
{
    [Export(typeof(IChannelActivator), Lifetime = ExportLifetime.Singleton)]
    public class ChannelActivator : IChannelActivator, IExecutor
    {
        /// <summary>
        /// Service Locator
        /// </summary>
        private IServiceLocator Services;

        /// <summary>
        /// Services that will get all classes with ChannelAttribute
        /// </summary>
        private IChannelLocator _channelLocator;

        /// <summary>
        /// Services that will give all info about the inspected ChannelMethod
        /// </summary>
        private IChannelMethodDescriptor _channelMethodDescriptor;

        /// <summary>
        /// Activates the ChannelMethod based on HttpMethod
        /// </summary>
        private IChannelMethodRequestActivator _requestActivator;

        /// <summary>
        /// Services responsible for writing output to the client
        /// </summary>
        private IChannelMessageService _msgService;

        /// <summary>
        /// Stopwatch for the benchmarks
        /// </summary>
        private Stopwatch watcher;

        /// <summary>
        /// Base URL for the Web Channels
        /// </summary>
        private string BaseURL = null;

        /// <summary>
        /// User defined Authentication Method Info
        /// </summary>
        public MethodInfo AuthMethod { get; set; }

        /// <summary>
        /// Class in which Authentication Method is defined
        /// </summary>
        public Type AuthMethodClass { get; set; }

        /// <summary>
        /// Authentication Method Delegate
        /// </summary>
        private Func<string, string, bool> _authenticationMethod;

        /// <summary>
        /// CTOR
        /// </summary>
        [DebuggerStepThrough]
        public ChannelActivator()
        {
            LogChannel.Write(LogSeverity.Info, "ChannelActivator Initialized");
            watcher = new Stopwatch();
        }

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="authMethodClass">Class in which Authentication Method is defined , method must take username and password as a parameters and must be of boolean return type</param>
        /// <param name="authMethod">User defined Authentication Method Info</param>
        [Obsolete("Please use second overload version of AuthenticationOptions method")]
        public void AuthenticationOptions(Type authMethodClass, MethodInfo authMethod)
        {
            AuthMethod = authMethod ?? throw new ArgumentNullException("Authentication method can not be null"); ;
            AuthMethodClass = authMethodClass ?? throw new ArgumentNullException("Class in which Authentication method is defined can not be null");

            if (authMethod.ReturnType != typeof(bool))
                throw new ChannelAuthException("Return type of authentication function must be of type bool");
        }

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="authMethod">Delegate for the authentication method</param>
        public void AuthenticationOptions(Func<string, string, bool> authMethod)
        {
            _authenticationMethod = authMethod ?? throw new ArgumentNullException("Authentication function must not be null");
        }

        /// <summary>
        /// Method that will do the initialization of Https
        /// </summary>
        /// <param name="domain">AppDomain with all assemblies</param>
        /// <param name="Services">IServiceLocator</param>
        public void Execute(AppDomain domain, IServiceLocator _Services, string baseURL = null)
        {
            Services = _Services;
            Debug.Assert(HttpListener.IsSupported);
            Debug.Assert(_Services != null);

            if (!HttpListener.IsSupported)
            {
                LogChannel.Write(LogSeverity.Fatal, "HttpListener not supported");
                LogChannel.Write(LogSeverity.Fatal, "Exiting ChannelActivator...");
                Console.WriteLine("HttpListener is not support on this platform");
                return;
            }
            if (baseURL != null)
                BaseURL = baseURL;

            _channelLocator = Services.Get<IChannelLocator>();
            _channelMethodDescriptor = Services.Get<IChannelMethodDescriptor>();
            _requestActivator = Services.Get<IChannelMethodRequestActivator>();
            _msgService = Services.Get<IChannelMessageService>();

            Debug.Assert(_channelLocator != null);
            Debug.Assert(_channelMethodDescriptor != null);
            Debug.Assert(_requestActivator != null);
            Debug.Assert(_msgService != null);

            List<Type> Channels = new List<Type>();

            Channels = _channelLocator.RegisteredChannels(domain);

            //Initialization part
            foreach (var channel in Channels)
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                Task task = new Task(() => MethodExecute(channel, token), token);
                task.Start();
            }
        }

        /// <summary>
        /// Method that will get all ChannelMethods from inspected Channel
        /// </summary>
        /// <param name="channel">Inspected Channel</param>
        public void MethodExecute(Type channel, CancellationToken cancellationToken)
        {
            MethodInfo[] methods = channel.GetMethods().Where(x => x.GetCustomAttribute(typeof(ChannelMethodAttribute)) != null).ToArray();
            foreach (var method in methods)
            {
                cancellationToken.ThrowIfCancellationRequested();
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                Task task = new Task(() => StartListening(method, channel, token), token);
                task.Start();
            }
        }

        /// <summary>
        /// Method that is doing all the heavy lifting, Http endpoint initialization for specified ChannelMethod
        /// </summary>
        /// <param name="method">ChannelMethod to be initialized as Http Endpoint</param>
        /// <param name="channel">Web Channel</param>
        public void StartListening(MethodInfo method, Type channel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ChannelEndpoint endpoint = new ChannelEndpoint();
            ChannelAttribute channelAttr = channel.GetCustomAttribute(typeof(ChannelAttribute)) as ChannelAttribute;
            if (!String.IsNullOrEmpty(channelAttr.Name))
                endpoint.URL = "/channels/" + channelAttr.Name + "/" + method.Name + "/";
            else
                endpoint.URL = "/channels/" + channel.Name + "/" + method.Name + "/";

            endpoint.Name = channel.Name + "." + method.Name;

            ChannelMethodAttribute ChannelMethod = method.GetCustomAttribute(typeof(ChannelMethodAttribute)) as ChannelMethodAttribute;
            AuthenticationSchemes ChannelSchema = ChannelMethod.Schema;
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
            if (ChannelSchema != AuthenticationSchemes.Anonymous)
            {
                httpChannel.AuthenticationSchemes = ChannelSchema;
                httpAuthRequired = true;
            }

            //Keep the ChannelMethod open for new requests
            while (true)
            {
                httpChannel.Start();

                Console.WriteLine($"Listening on {methodURL}");

                HttpListenerContext context = httpChannel.GetContext();
                HttpListenerRequest request = context.Request;

                LogChannel.Write(LogSeverity.Info, $"Request coming to {endpoint.Name}");
                LogChannel.Write(LogSeverity.Info, $"HttpMethod:{request.HttpMethod}");
                LogChannel.Write(LogSeverity.Info, $"IsAuthenticated:{request.IsAuthenticated}");

                HttpListenerResponse response = context.Response;

                //Check if the Http Method is correct
                if (HttpMethod.ToString() != request.HttpMethod && HttpMethod != ChannelHttpMethod.Unknown)
                {
                    IChannelMessage msg = new ChannelMessage()
                    {
                        Message = $"Wrong HTTP Method used. In order to call this endpoint u need to send {HttpMethod.ToString()} request"
                    };
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    LogChannel.Write(LogSeverity.Error, "Wrong HTTP Method used");
                    string outputString = JsonConvert.SerializeObject(msg, Formatting.Indented);
                    using (StreamWriter writer = new StreamWriter(response.OutputStream))
                    {
                        writer.WriteLine(outputString);
                    }
                    response.Close();
                }


                AuthorizeChannelAttribute authAttr = channel.GetCustomAttribute(typeof(AuthorizeChannelAttribute)) as AuthorizeChannelAttribute;
                bool authenticated = false;
                if (authAttr != null)
                {
                    AuthenticateRequest(context, response, ChannelSchema, out authenticated);
                }
                if (httpAuthRequired)
                {
                    AuthenticateRequest(context, response, ChannelSchema, out authenticated);
                }


                Dictionary<string, Type> methodDescription = _channelMethodDescriptor.GetMethodDescription(method);
                List<object> channelRequestBody = null;

                if (request.HttpMethod == "GET" && request.QueryString.AllKeys.Length > 0)
                    _requestActivator.GetActivateWithParameters(channel, method, channelRequestBody, methodDescription, request, response);
                else if (request.HttpMethod == "GET" && request.QueryString.AllKeys.Length == 0)
                    _requestActivator.GetActivateWithoutParameters(channel, method, response);

                //Enter only if Request Body is supplied with POST Method
                if (request.HasEntityBody == true && request.HttpMethod == "POST")
                {
                    try
                    {
                        _requestActivator.PostActivate(channel, method, channelRequestBody, methodDescription, request, response);
                    }
                    catch(ChannelMethodParameterException pEx)
                    {
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _msgService.ExceptionHandler(writer, pEx, response);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (ChannelMethodContentTypeException cEx)
                    {
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _msgService.ExceptionHandler(writer, cEx, response);
                        writer.Flush();
                        writer.Close();
                    }
                    catch(ArgumentException aEx)
                    {
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _msgService.ExceptionHandler(writer, aEx, response);
                        writer.Flush();
                        writer.Close();
                    }
                }

                LogChannel.Write(LogSeverity.Debug, "Request finished...");
                LogChannel.Write(LogSeverity.Debug, "Closing the response");
                response.Close();
            }

        }

        /// <summary>
        /// Authenticating Request based on AuthType
        /// </summary>
        /// <param name="context">HttpListenerContext</param>
        /// <param name="response">HttpListenerResponse for the client</param>
        /// <param name="ChannelSchema">AuthenticationSchemes for the Channel</param>
        public void AuthenticateRequest(HttpListenerContext context, HttpListenerResponse response, AuthenticationSchemes ChannelSchema, out bool authenticated)
        {
            if (!context.Request.IsAuthenticated || context.User == null)
            {
                _msgService.FailedAuthenticationResponse(ChannelSchema, response);
                authenticated = false;
            }
            else
            {
                authenticated = true;
                if (context.User.Identity is HttpListenerBasicIdentity)
                {
                    try
                    {
                        HttpListenerBasicIdentity basicIdentity = context.User.Identity as HttpListenerBasicIdentity;
                        LogChannel.Write(LogSeverity.Info, $"{basicIdentity.Name} trying to reach channel");
                        bool success = _authenticationMethod.Invoke(basicIdentity.Name, basicIdentity.Password);
                        if (success)
                            LogChannel.Write(LogSeverity.Info, $" Authentication for {basicIdentity.Name} succeded");
                        if (!success)
                        {
                            _msgService.FailedAuthenticationResponse(ChannelSchema, response);
                            LogChannel.Write(LogSeverity.Info, $" Authentication for {basicIdentity.Name} failed");
                            authenticated = false;
                        }
                        else
                            authenticated = true;
                    }
                    catch (Exception ex)
                    {
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _msgService.ExceptionHandler(writer, ex, response);
                        writer.Flush();
                        writer.Close();
                    }

                }
                else
                    _msgService.FailedAuthenticationResponse(ChannelSchema, response);
            }
        }
    }


}
