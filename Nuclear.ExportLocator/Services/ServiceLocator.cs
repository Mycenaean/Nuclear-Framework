// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Microsoft.Extensions.DependencyInjection;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nuclear.ExportLocator.Extensions")]
namespace Nuclear.ExportLocator.Services
{
    /// <summary>
    /// Service Locator Container
    /// </summary>
    internal sealed class ServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _provider;
        private readonly IServiceCollection _services;
        private readonly ExportFactory _factory;
        private IList<ExportInformation> _exports;
        private static ServiceLocator Locator = null;
        private static readonly object _lock = new object();

        internal IServiceCollection Services => _services;

        /// <summary>
        /// Get ServiceLocator instance
        /// </summary>
        public static ServiceLocator GetInstance
        {
            get
            {
                lock (_lock)
                {
                    if (Locator == null)
                    {
                        Locator = new ServiceLocator();
                        return Locator;
                    }
                    else
                        return Locator;
                }
            }
        }

        /// <summary>
        /// Private constructor for Singleton Design Pattern
        /// </summary>
        private ServiceLocator()
        {
            _factory = new ExportFactory();
            _services = new ServiceCollection();
            _exports = new List<ExportInformation>();
            _provider = InitializeServices();
        }

        internal ServiceLocator(List<string> assemblies)
        {
            _factory = new ExportFactory(assemblies.ToArray());
            _services = new ServiceCollection();
            _exports = new List<ExportInformation>();
            _provider = InitializeServices();
            Locator = this;
        }

        /// <summary>
        /// Get all services with Export attribute
        /// </summary>
        /// <returns>IServiceProvider Instance</returns>
        private IServiceProvider InitializeServices()
        {
            _exports = _factory.GetExports();
            foreach (var export in _exports)
            {
                switch (export.ExportLifetime)
                {
                    case ExportLifetime.Scoped:
                        _services.AddScoped(export.ServiceType, export.Implementation);
                        break;
                    case ExportLifetime.Transient:
                        _services.AddTransient(export.ServiceType, export.Implementation);
                        break;
                    case ExportLifetime.Singleton:
                        _services.AddSingleton(export.ServiceType, export.Implementation);
                        break;
                    default:
                        _services.AddTransient(export.ServiceType, export.Implementation);
                        break;
                }
            }
            return _services.BuildServiceProvider();
        }

        /// <summary>
        /// Get the service from IServiceLocator
        /// </summary>
        /// <typeparam name="T">Requested Interface</typeparam>
        /// <returns>Specified service</returns>
        public T Get<T>()
        {
            Debug.Assert(_provider.GetService<T>() != null);
            return _provider.GetService<T>();
        }

        /// <summary>
        /// Get the service from IServiceLocator
        /// </summary>
        /// <param name="service">Requested Type</param>
        /// <returns>Specified service as an object</returns>
        public object GetObject(Type service)
        {   
            Debug.Assert(_provider.GetService(service) != null);
            return _provider.GetService(service);
        }
    }
}
