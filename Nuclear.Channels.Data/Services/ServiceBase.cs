// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Data.Services
{
    public abstract class ServiceBase
    {
        protected readonly IServiceLocator _services;

        protected ServiceBase()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
        }
    }
}
