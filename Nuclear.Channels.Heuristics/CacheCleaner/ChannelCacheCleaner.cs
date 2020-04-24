// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Heuristics.EventArguments;
using Nuclear.Channels.Heuristics.Events;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;

namespace Nuclear.Channels.Heuristics.CacheCleaner
{
    [Export(typeof(IChannelCacheCleaner), ExportLifetime.Singleton)]
    internal class ChannelCacheCleaner : IChannelCacheCleaner
    {
        private readonly IChannelHeuristicEvents _events;
        private readonly IServiceLocator _services;
        private readonly IChannelHeuristics _heuristics;
        private Timer _timer;

        public ChannelCacheCleaner()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _heuristics = _services.Get<IChannelHeuristics>();
            _events = _services.Get<IChannelHeuristicEvents>();
        }

        public void CollectExpired(TimeSpan interval)
        {
            _timer = new Timer(Collect, null, TimeSpan.Zero, interval);
        }

        [SuppressMessage("Nullable.Reference.Warning", "CS8632", Justification = "Needed for Timer constructor")]
        public void Collect(object? state)
        {
            HeuristicsInfo[] expired = _heuristics.GetExpiredCache();
            if (expired.Length == 0)
                return;

            RemoveHeuristicsEventArgs removeArgs = new RemoveHeuristicsEventArgs
            {
                CollectionForRemoval = expired
            };

            _events.OnRemoveFromHeuristics(removeArgs);
        }
    }
}
