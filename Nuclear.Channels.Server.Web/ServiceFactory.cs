// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Web
{
    /// <summary>
    /// Wrapper around IServiceLocator
    /// </summary>
    public static class ServiceFactory
    {
        private static IServiceLocator _services = ServiceLocatorBuilder.CreateServiceLocator();

        public static TService GetExportedService<TService>()
        {
            return _services.Get<TService>();
        }
    }
}
