using Microsoft.Extensions.DependencyInjection;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nuclear.ExportLocator.Services
{
    /// <summary>
    /// Service Locator Container
    /// </summary>
    internal sealed class ServiceLocator : IServiceLocator
    {
        private IServiceProvider provider;
        private IServiceCollection services;
        private ExportFactory factory;
        private IList<ExportInformation> exports;
        private static ServiceLocator Locator = null;

        /// <summary>
        /// Get ServiceLocator singleton instance
        /// </summary>
        public static ServiceLocator GetInstance
        {
            get
            {
                if (Locator == null)
                {
                    Locator = new ServiceLocator();
                    return new ServiceLocator();
                }
                else
                    return Locator;
            }
        }

        /// <summary>
        /// Private constructor for Singletone Design Pattern
        /// </summary>
        private ServiceLocator()
        {
            factory = new ExportFactory();
            services = new ServiceCollection();
            exports = new List<ExportInformation>();
            provider = InitializeServices();
        }

        /// <summary>
        /// Get all services with Export attribute
        /// </summary>
        /// <returns>IServiceProvider Instance</returns>
        private IServiceProvider InitializeServices()
        {
            exports = factory.GetExports();
            foreach (var export in exports)
            {
                if (export.ExportLifetime == ExportLifetime.Scoped)
                    services.AddScoped(export.ServiceType, export.Implementation);
                else if (export.ExportLifetime == ExportLifetime.Transient)
                    services.AddTransient(export.ServiceType, export.Implementation);
                else if (export.ExportLifetime == ExportLifetime.Singleton)
                    services.AddSingleton(export.ServiceType, export.Implementation);
                else
                    services.AddTransient(export.ServiceType, export.Implementation);

            }
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Get the service from IServiceLocator
        /// </summary>
        /// <typeparam name="T">Requested Interface</typeparam>
        /// <returns>Specified service</returns>
        public T Get<T>()
        {
            Debug.Assert(provider.GetService<T>() != null);
            return provider.GetService<T>();
        }
    }
}
