using Nuclear.Channels.Hosting;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Base
{
    /// <summary>
    /// Implementation of ChannelHosting
    /// </summary>
    internal class ChannelHost : IChannelHost
    {
        private AppDomain _domain;
        private IServiceLocator Services;
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
                lock(_lock)
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
            Services = ServiceLocatorBuilder.CreateServiceLocator();
            _activator = Services.Get<IChannelActivator>();
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
            _activator.Execute(_domain, Services, baseURL);
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
