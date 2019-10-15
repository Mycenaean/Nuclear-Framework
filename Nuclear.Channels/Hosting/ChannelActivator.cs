using Newtonsoft.Json;
using Nuclear.Channels.Auth;
using Nuclear.Channels.Auth.Identity;
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
        /// Authentication Method Delegate for Basic authentication
        /// </summary>
        private Func<string, string, bool> _basicAuthenticationMethod;

        /// <summary>
        /// Authentication Method Delegate for Token authentication
        /// </summary>
        private Func<string, bool> _tokenAuthenticationMethod;

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
        /// <param name="authMethod">Delegate for the authentication method</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AuthenticationOptions(Func<string, string, bool> basicAuthMethod)
        {
            _basicAuthenticationMethod = basicAuthMethod ?? throw new ArgumentNullException("Authentication function must not be null");
        }

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="authMethod">Delegate for the authentication method</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AuthenticationOptions(Func<string, bool> tokenAuthMethod)
        {
            _tokenAuthenticationMethod = tokenAuthMethod ?? throw new ArgumentNullException("Authentication function must not be null");
        }

        /// <summary>
        /// Method that will do the initialization of Https
        /// </summary>
        /// <param name="domain">AppDomain with all assemblies</param>
        /// <param name="Services">IServiceLocator</param>
        /// <exception cref="HttpListenerNotSupportedException"></exception>
        public void Execute(AppDomain domain, IServiceLocator _Services, string baseURL = null)
        {
            Services = _Services;
            Debug.Assert(HttpListener.IsSupported);
            Debug.Assert(_Services != null);

            if (!HttpListener.IsSupported)
            {
                LogChannel.Write(LogSeverity.Fatal, "HttpListener not supported");
                LogChannel.Write(LogSeverity.Fatal, "Exiting ChannelActivator...");
                throw new HttpListenerNotSupportedException("HttpListener is not supported");
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
        /// <param name="channel">Channel</param>
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
            AuthorizeChannelAttribute authAttr = channel.GetCustomAttribute(typeof(AuthorizeChannelAttribute)) as AuthorizeChannelAttribute;
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
            if (ChannelSchema != ChannelAuthenticationSchemes.Anonymous)
            {
                httpAuthRequired = true;
            }

            //Keep the ChannelMethod open for new requests
            while (true)
            {
                httpChannel.Start();
                Console.WriteLine($"Initialized {methodURL}");
                HttpListenerContext context = httpChannel.GetContext();
                HttpListenerRequest request = context.Request;

                LogChannel.Write(LogSeverity.Info, $"Request coming to {endpoint.Name}");
                LogChannel.Write(LogSeverity.Info, $"HttpMethod:{request.HttpMethod}");
                LogChannel.Write(LogSeverity.Info, $"IsAuthenticated:{request.IsAuthenticated}");

                HttpListenerResponse response = context.Response;

                if(httpAuthRequired)
                {
                    HttpListenerIdentityService identityService = new HttpListenerIdentityService(_basicAuthenticationMethod, _tokenAuthenticationMethod);
                    StreamWriter writer = new StreamWriter(response.OutputStream);
                    try
                    {
                        bool knownUser = identityService.AuthenticatedAndAuthorized(context, ChannelSchema);
                        if (!knownUser)
                            _msgService.FailedAuthenticationResponse(ChannelSchema, response);
                    }
                    catch(ChannelCredentialsException cEx)
                    {
                        _msgService.ExceptionHandler(writer, cEx, response);
                    }
                    catch(HttpListenerException hEx)
                    {
                        _msgService.ExceptionHandler(writer, hEx, response);
                    }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }                    
                }

                //Check if the Http Method is correct
                if (HttpMethod.ToString() != request.HttpMethod && HttpMethod != ChannelHttpMethod.Unknown)
                {
                    _msgService.WrongHttpMethod(response, HttpMethod);
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
                    StreamWriter writer = new StreamWriter(response.OutputStream);
                    try
                    {
                        _requestActivator.PostActivate(channel, method, channelRequestBody, methodDescription, request, response);
                    }
                    catch (ChannelMethodContentTypeException cEx)
                    {
                        _msgService.ExceptionHandler(writer, cEx, response);
                    }
                    catch (ChannelMethodParameterException pEx)
                    {
                        _msgService.ExceptionHandler(writer, pEx, response);
                    }
                    catch (TargetParameterCountException tEx)
                    {
                        _msgService.ExceptionHandler(writer, tEx, response);
                    }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }

                LogChannel.Write(LogSeverity.Debug, "Request finished...");
                LogChannel.Write(LogSeverity.Debug, "Closing the response");
                response.Close();
            }

        }
    }


}
