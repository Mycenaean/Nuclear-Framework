// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IServiceLocator _services;
        private readonly IChannelActivator _activator;
        private static ChannelHost _host;
        private static readonly object _lock = new object();
        private List<string> _lookupAssemblies = null;

        public AuthenticationSettings AuthenticationSettings { get; set; }

        private static readonly List<string> KnownExportAssemblies = new List<string>()
        {
            "Nuclear.Channels",
            "Nuclear.Channels.Base",
            "Nuclear.Channels.Authentication",
            "Nuclear.Channels.Generators",
            "Nuclear.Channels.Heuristics",
            "Nuclear.Channels.InvokerServices",
            "Nuclear.Channels.Messaging",
            "Nuclear.Channels.Data",
        };

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
            AuthenticationSettings = null;
        }

        private IServiceLocator RegisterServices()
        {
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Base");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Messaging");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.InvokerServices");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Authentication");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Data");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Generators");
            AppDomain.CurrentDomain.Load("Nuclear.Channels.Heuristics");

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
            if (_lookupAssemblies != null)
                _activator.Execute(_lookupAssemblies, _services, AuthenticationSettings, baseURL);
            else
                _activator.Execute(_domain, _services, AuthenticationSettings, baseURL);

            Task.WaitAll();
        }

        public void IsServerManaged(bool managed = false)
        {
            _activator.IsServerManaged(managed);
        }

        public void AuthenticationOptions(Func<string, bool> tokenAuthenticationMethod)
        {
            _activator.AuthenticationOptions(tokenAuthenticationMethod);
        }

        public void AuthenticationOptions(Func<string, string, bool> basicAuthenticationMethod)
        {
            _activator.AuthenticationOptions(basicAuthenticationMethod);
        }

        public void RegisterChannels(List<string> assemblieContainingChannels)
        {
            if (assemblieContainingChannels != null && assemblieContainingChannels.Any())
                _lookupAssemblies = assemblieContainingChannels;                
        }
    }
}
