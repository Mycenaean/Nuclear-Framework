// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Heuristics.CacheCleaner
{
    public static class ChannelCacheExtensions
    {
        private static IServiceLocator _services = ServiceLocatorBuilder.CreateServiceLocator();
        private static IChannelCacheCleaner _cleaner = _services.Get<IChannelCacheCleaner>();
        public static IChannelCacheCleanable ConfigureCacheCleaner(this IChannelCacheCleanable server,TimeSpan interval)
        {
            _cleaner.CollectExpired(interval);
            return server;
        }
    }
}
