// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Microsoft.Win32.SafeHandles;
using Nuclear.Channels.Authentication;
using Nuclear.Channels.Base;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Data.Logging;
using Nuclear.Channels.Heuristics;
using Nuclear.Channels.Heuristics.Contexts;
using Nuclear.Channels.InvokerServices.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Nuclear.Channels.Handlers
{
    public class ChannelMethodHandler : IHandlerContract , IServerManaged , IDisposable
    {
        private readonly IServiceLocator _services;
        private readonly IChannelMethodDescriptor _channelMethodDescriptor;
        private readonly IChannelMethodRequestActivator _requestActivator;
        private readonly IChannelMessageService _msgService;
        private readonly IChannelMethodContextProvider _contextProvider;
        private readonly IChannelConfiguration _configuration;
        private readonly IChannelAuthenticationService _authenticationService;
        private readonly IChannelHeuristics _heuristics;
        private readonly ISessionService _session;
        private AuthenticationSettings _settings;
        private Func<string, string, bool> _basicAuthenticationMethod;
        private Func<string, bool> _tokenAuthenticationMethod;
        private HttpListener _httpListener;
        private MethodInfo _method;
        private Type _channel;
        private string _baseURL;
        private bool _isManaged;
        private bool _isDisposed;
        private SafeHandle _safeHandle;

        public EntityState State { get; private set; }
        public string StateName { get; private set; }
        public string HandlerId { get;  }
        public string ChannelHandlerId { get; }
        public string Url { get; private set; }


        public ChannelMethodHandler(IServiceLocator services, Type channel, MethodInfo method, AuthenticationSettings settings, string baseURL, string channelHandlerId)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _settings = settings;
            _baseURL = baseURL;
            _isManaged = false;

            _channelMethodDescriptor = _services.Get<IChannelMethodDescriptor>();
            _requestActivator = _services.Get<IChannelMethodRequestActivator>();
            _msgService = _services.Get<IChannelMessageService>();
            _contextProvider = _services.Get<IChannelMethodContextProvider>();
            _configuration = _services.Get<IChannelConfiguration>();
            _authenticationService = _services.Get<IChannelAuthenticationService>();
            _heuristics = _services.Get<IChannelHeuristics>();
            _session = _services.Get<ISessionService>();

            HandlerId = $"{Guid.NewGuid()}";
            ChannelHandlerId = channelHandlerId;

            _isDisposed = false;
            _safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        }

        public void SetupBasicAuth(Func<string, string, bool> basicAuth)
        {
            _basicAuthenticationMethod = basicAuth;
        }

        public void SetupTokenAuth(Func<string, bool> tokenAuth)
        {
            _tokenAuthenticationMethod = tokenAuth;
        }

        public void IsServerManaged(bool managed = false)
        {
            _isManaged = managed;
        }

        public void Start()
        {
            State = EntityState.Starting;
            StateName = Enum.GetName(typeof(EntityState), EntityState.Starting);
            
            _httpListener = new HttpListener();
            ChannelConfigurationInfo channelConfig = _configuration.Configure(_httpListener, _channel, _method, _baseURL);

            Url = channelConfig.MethodUrl;
            //Keep the ChannelMethod open for new requests
            while (true)
            {
                try
                {
                    _httpListener.Start();
                    State = EntityState.Running;
                    StateName = Enum.GetName(typeof(EntityState), EntityState.Running);
                }
                catch (HttpListenerException hle)
                {
                    Console.WriteLine($"System.Net.HttpListenerException encountered on {channelConfig.MethodUrl} with reason :{hle.Message}");
                    return;
                }

                if(!_isManaged)
                    Console.WriteLine($"Listening on {Url}");

                HttpListenerContext context = _httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                IChannelHeuristicContext heuristicsCtx = _services.Get<IChannelHeuristicContext>();

                LogChannel.Write(LogSeverity.Info, $"Request coming to {channelConfig.Endpoint.Name}");
                LogChannel.Write(LogSeverity.Info, $"HttpMethod:{request.HttpMethod}");

                ChannelAuthenticationInspector authInspector = new ChannelAuthenticationInspector(_authenticationService, _msgService, _settings, _session, _basicAuthenticationMethod, _tokenAuthenticationMethod);

                //Even if method is cached check authenticaion first
                bool authFailed = authInspector.AuthenticationFailedIfRequired(context, request, response, channelConfig, out bool authenticated);
                if (authFailed)
                    goto EndRequest;

                List<object> channelRequestBody = null;

                ChannelMethodCacheInspector cacheInspector = new ChannelMethodCacheInspector(_msgService, _heuristics);
                CacheExecutionResult cacheExecutionResult = cacheInspector.ExecuteIfCached(_channel, _method, request, response, heuristicsCtx);

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

                ChannelMethodInfo methodDescription = _channelMethodDescriptor.GetMethodDescription(_method);
                ChannelMethodCaller channelMethodCaller = new ChannelMethodCaller(_msgService, _contextProvider, _requestActivator);

                if (request.HttpMethod == "GET")
                {
                    channelMethodCaller.TryInvokeGetRequest(_channel, _method, request, response, methodDescription, channelConfig, channelRequestBody, authenticated, cacheExecutionResult);
                    goto EndRequest;
                }

                //Enter only if Request Body is supplied with POST Method
                if (request.HasEntityBody == true && request.HttpMethod == "POST")
                {
                    channelMethodCaller.TryInvokePostRequest(_channel, _method, request, response, channelRequestBody, methodDescription, channelConfig, authenticated, cacheExecutionResult);
                }

            EndRequest:
                LogChannel.Write(LogSeverity.Debug, "Request finished...");
                LogChannel.Write(LogSeverity.Debug, "Closing the response");
                response.Close();
            }
        }

        public void Restart()
        {
            if (_httpListener.IsListening)
            {
                _httpListener.Stop();
                _httpListener = null;
            }
            State = EntityState.Restarting;
            StateName = Enum.GetName(typeof(EntityState), EntityState.Restarting);
            Start();
        }

        public void Stop()
        {
            if (_httpListener.IsListening)
                _httpListener.Stop();

            State = EntityState.Stopped;
            StateName = Enum.GetName(typeof(EntityState), EntityState.Stopped);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _safeHandle?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
