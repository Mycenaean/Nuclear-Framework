using Nuclear.Channels.Heuristics.EventArguments;
using Nuclear.Channels.Heuristics.Events;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics.Contexts
{
    [Export(typeof(IChannelHeuristicContext), ExportLifetime.Singleton)]
    public class ChannelHeuristicContext : IChannelHeuristicContext
    {
        private readonly IServiceLocator _services;
        private readonly IChannelHeuristicEvents _events;

        public ChannelHeuristicContext()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _events = _services.Get<IChannelHeuristicEvents>();
        }

        public bool ExpectsAdding { get; set; }
        public Type Channel { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public List<object> Parameters { get; set; }
        public object MethodResponse { get; set; }

        public void CacheResponse()
        {
            AddHeuristicsEventArgs eventArgs = new AddHeuristicsEventArgs
            {
                Channel = Channel,
                MethodInfo = MethodInfo,
                Parameters = Parameters,
                MethodResponse = MethodResponse
            };

            _events.OnAddToHeuristics(eventArgs);
        }

        public void Clear()
        {
            Channel = null;
            MethodInfo = null;
            MethodResponse = null;
            Parameters = null;
            ExpectsAdding = false;
        }
    }
}
