// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channel.Generators.Exceptions;
using Nuclear.Channels.Generators;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nuclear.Channel.Generators
{
    [Export(typeof(IImportedServicesResolver), ExportLifetime.Transient)]
    public class ImportResolver : IImportedServicesResolver
    {
        private readonly IServiceLocator _services;

        public ImportResolver()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
        }
        
        public object GetImportedService(Type reqService)
        {
            object service = _services.GetObject(reqService);
            if (service == null)
                throw new ImportFailedException($"No service registered for type {reqService}");
            else
                return service;

        }
    }
}
