// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels
{
    /// <summary>
    /// Implementation of ChannelHosting
    /// </summary>
    internal class ChannelHost : IChannelServer
    {
        private AppDomain _domain;
        private IServiceLocator _services;
        private IChannelActivator _activator;
        private static ChannelHost _host;
        private static object _lock = new object();

        /// <summary>
        /// Get the Singleton Instance
        /// </summary>
        public static ChannelHost GetHost
        {
            get
            {
                lock (_lock)
                {
                    if (_host == null)
                    {
                        _host = new ChannelHost();
                    }
                    return _host;
                }
            }
        }

        /// <summary>
        /// Singleton Design pattern private constructor
        /// </summary>
        private ChannelHost()
        {
            _services = RegisterServices();
            _activator = _services.Get<IChannelActivator>();
        }

        private IServiceLocator RegisterServices()
        {
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Base");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Messaging");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.InvokerServices");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Authentication");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Data");

            return ServiceLocatorBuilder.CreateServiceLocator();
        }

        /// <summary>
        /// Method to load all assemblies containing channels
        /// </summary>
        /// <param name="domain">Current AppComain</param>
        /// <param name="assemblies">Assembly names of your projects</param>
        public void LoadAssemblies(AppDomain domain, string[] assemblies)
        {
            _domain = domain;
            if (assemblies != null && assemblies.Length > 0)
            {
                foreach (var assembly in assemblies)
                {
                    _domain.Load(assembly);
                }
            }
        }

        /// <summary>
        /// Load App Domain
        /// </summary>
        /// <param name="domain">Current AppDomain</param>
        public void LoadAssemblies(AppDomain domain)
        {
            _domain = domain;
        }

        /// <summary>
        /// Starts hosting
        /// </summary>
        public void StartHosting(string baseURL)
        {
            _activator.Execute(_domain, _services, baseURL);
            Task.WaitAll();
        }

        public void AuthenticationOptions(Func<string, bool> tokenAuthenticationMethod)
        {
            _activator.AuthenticationOptions(tokenAuthenticationMethod);
        }

        public void AuthenticationOptions(Func<string, string, bool> basicAuthenticationMethod)
        {
            _activator.AuthenticationOptions(basicAuthenticationMethod);
        }
    }
}
