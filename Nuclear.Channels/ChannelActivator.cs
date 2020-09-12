// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.Channels.Base.Contracts;
using Nuclear.Channels.Base.Exceptions;
using Nuclear.Channels.Data.Logging;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Handlers;
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
        private AuthenticationSettings _settings;
        private IChannelMethodHandlerCollection _methodHandlers;
        private IChannelHandlerCollection _channelHandlers;
        private Func<string, string, bool> _basicAuthenticationMethod;
        private Func<string, bool> _tokenAuthenticationMethod;
        private string _rootPath;
        private string _baseURL = null;
        private bool _isManaged;

        private static object _lock;


        [DebuggerStepThrough]
        public ChannelActivator()
        {
            _lock = new object();
            _isManaged = false;
        }

        public void AuthenticationOptions(Func<string, string, bool> basicAuthMethod)
        {
            _basicAuthenticationMethod = basicAuthMethod ?? throw new ArgumentNullException("Authentication function must not be null");
        }

        public void AuthenticationOptions(Func<string, bool> tokenAuthMethod)
        {
            _tokenAuthenticationMethod = tokenAuthMethod ?? throw new ArgumentNullException("Authentication function must not be null");
        }

        public void IsServerManaged(bool managed = false)
        {
            _isManaged = managed;
        }

        public IExecutor GetRawExecutor()
        {
            return this;
        }

        private void OneTimeSetup(IServiceLocator services)
        {
            Debug.Assert(services != null);
            Debug.Assert(HttpListener.IsSupported);

            _services = services;
            _channelLocator = _services.Get<IChannelLocator>();
            _methodHandlers = _services.Get<IChannelMethodHandlerCollection>();
            _channelHandlers = _services.Get<IChannelHandlerCollection>();

            _rootPath = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName;
            DirectoryInfo logsDirectory = new DirectoryInfo(Path.Combine(_rootPath, "Logs"));
            if (!Directory.Exists(logsDirectory.FullName))
                logsDirectory.Create();
        }

        public void Execute(List<string> lookupAssemblies, IServiceLocator services, AuthenticationSettings settings, string baseURL = null)
        {
            OneTimeSetup(services);
            List<Type> channels = _channelLocator.RegisteredChannels(lookupAssemblies);
            Execute(settings, channels, baseURL);
        }

        public void Execute(AppDomain domain, IServiceLocator _Services, AuthenticationSettings settings, string baseURL = null)
        {
            OneTimeSetup(_Services);
            List<Type> channels = _channelLocator.RegisteredChannels(domain);
            Execute(settings, channels, baseURL);
        }

        public void Execute(AuthenticationSettings settings, List<Type> channelAssemblies, string baseURL = null)
        {
            _settings = settings;

            if (!HttpListener.IsSupported)
            {
                LogChannel.Write(LogSeverity.Fatal, "HttpListener not supported");
                LogChannel.Write(LogSeverity.Fatal, "Exiting ChannelActivator...");
                throw new HttpListenerNotSupportedException("HttpListener is not supported");
            }
            if (baseURL != null)
                _baseURL = baseURL;

            //Initialization part
            foreach (Type channel in channelAssemblies)
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                Task task = new Task(() => MethodExecute(channel, token), token);
                task.Start();
            }
        }

        public void MethodExecute(Type channel, CancellationToken cancellationToken)
        {
            MethodInfo[] methods = ChannelMethodReflector.GetChannelMethods(channel);

            ChannelHandler channelHandler = new ChannelHandler(channel.Name);

            //lock the list for safety
            lock (_lock)
            {
                _channelHandlers.AddHandler(channelHandler);
            }

            foreach (MethodInfo method in methods)
            {
                //Exception should be thrown to developer if EnableCache is on top of void Method
                ChannelMethodCacheInspector.CheckCacheValidity(method);

                cancellationToken.ThrowIfCancellationRequested();
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                Task task = new Task(() => StartListening(method, channel, channelHandler.HandlerId, token), token);
                task.Start();
            }
        }

        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "Used")]
        [SuppressMessage("Style", "IDE0049:Name can be simplified", Justification = "String is a class")]
        public void StartListening(MethodInfo method, Type channel, string channelHandlerId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ChannelMethodHandler methodHandler = new ChannelMethodHandler(_services, channel, method, _settings, _baseURL, channelHandlerId);

            if (_basicAuthenticationMethod != null) methodHandler.SetupBasicAuth(_basicAuthenticationMethod);
            if (_tokenAuthenticationMethod != null) methodHandler.SetupTokenAuth(_tokenAuthenticationMethod);
            methodHandler.IsServerManaged(_isManaged);

            //lock the list for safety
            lock (_lock)
            {
                _methodHandlers.AddHandler(methodHandler);
            }

            methodHandler.Start();
        }

    }
}
