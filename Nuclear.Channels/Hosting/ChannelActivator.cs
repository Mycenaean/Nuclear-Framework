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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Formatting = Newtonsoft.Json.Formatting;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Hosting
{
    [Export(typeof(IChannelActivator), Lifetime = ExportLifetime.Singleton)]
    public class ChannelActivator : IChannelActivator, IExecutor
    {
        private IServiceLocator Services;
        private IChannelLocator _channelLocator;
        private IChannelMethodDescriptor _channelMethodDescriptor;
        private IChannelMethodRequestActivator _requestActivator;
        private IChannelMessageService _msgService;
        private Stopwatch watcher;
        private string BaseURL = null;
        private Func<string, string, bool> _basicAuthenticationMethod;
        private Func<string, bool> _tokenAuthenticationMethod;

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

                if (httpAuthRequired)
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
                    }
                    catch (ChannelCredentialsException cEx)
                    {
                        response.StatusCode = 401;
                        _msgService.ExceptionHandler(writer, cEx, response);
                    }
                    catch (HttpListenerException hEx)
                    {
                        _msgService.ExceptionHandler(writer, hEx, response);
                    }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }

                    if(!authenticated)
                        goto AuthenticationFailed;
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
                        response.StatusCode = 400;
                        _msgService.ExceptionHandler(writer, cEx, response);
                    }
                    catch (ChannelMethodParameterException pEx)
                    {
                        response.StatusCode = 400;
                        _msgService.ExceptionHandler(writer, pEx, response);
                    }
                    catch (TargetParameterCountException tEx)
                    {
                        response.StatusCode = 400;
                        _msgService.ExceptionHandler(writer, tEx, response);
                    }
                    finally
                    {
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
    }
}
