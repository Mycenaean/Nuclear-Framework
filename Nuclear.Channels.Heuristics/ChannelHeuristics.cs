// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using Nuclear.Channels.Data.Deserializers;
using Nuclear.Channels.Heuristics.EventArguments;
using Nuclear.Channels.Heuristics.Events;
using Nuclear.Channels.Messaging;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Nuclear.Channels.Heuristics
{
    /// <summary>
    /// Purpose of this service is to write cached response without invoking already registered Channel
    /// </summary>
    [Export(typeof(IChannelHeuristics), ExportLifetime.Singleton)]
    internal class ChannelHeuristics : IChannelHeuristics
    {
        private readonly IServiceLocator _services;
        private readonly IChannelMessageService _msgService;
        private readonly IChannelMethodDescriptor _descriptor;
        private readonly IChannelHeuristicEvents _events;
        private static List<HeuristicsInfo> _cachedInfos;
        private Timer _timer;
        private static object _lock = new object();

        public ChannelHeuristics()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _descriptor = _services.Get<IChannelMethodDescriptor>();
            _msgService = _services.Get<IChannelMessageService>();
            _events = _services.Get<IChannelHeuristicEvents>();

            _events.AddToHeuristics += _events_AddToHeuristics;
            _events.RemoveFromHeuristics += _events_RemoveFromHeuristics;
            _cachedInfos = new List<HeuristicsInfo>();

            //timer to clear cache every 5 minutes
            _timer = new Timer(CollectExpiredCacheObject, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        public bool IsMethodCached(Type channel, MethodInfo channelMethod, out HeuristicsInfo hInfo)
        {
            lock (_lock)
            {
                HeuristicsInfo cached = _cachedInfos.FirstOrDefault(x => x.Channel == channel && x.ChannelMethod == channelMethod);

                if (cached == null)
                {
                    hInfo = null;
                    return false;
                }

                if (cached.Expired())
                {
                    hInfo = null;

                    _cachedInfos.Remove(cached);
                    return false;
                }
                else
                {
                    hInfo = cached;
                    return true;
                }
            }
        }

        public bool Execute(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo)
        {
            string method = options.Request.HttpMethod;

            if (method == "GET")
            {
                return TryInvokeGetMethod(options, hInfo);
            }
            else
            {
                return TryInvokePostMethod(options, hInfo);
            }
        }

        private bool TryInvokeGetMethod(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo)
        {
            if (options.Request.QueryString.AllKeys.Length == 0)
            {
                _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                return true;
            }
            else
            {
                ChannelMethodDeserializerFactory dsrFactory = new ChannelMethodDeserializerFactory(options.Request.QueryString);
                List<object> requestParameters = dsrFactory.DeserializeFromQueryParameters(_descriptor.GetMethodDescription(options.ChannelMethod));

                if (requestParameters == hInfo.Parameters)
                {
                    _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                    return true;
                }
                else
                    return false;
            }

        }

        private bool TryInvokePostMethod(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo)
        {
            if (options.Request.HasEntityBody)
            {
                ChannelMethodDeserializerFactory dsrFactory = new ChannelMethodDeserializerFactory(options.Request.InputStream);
                List<object> requestParameters = dsrFactory.DeserializeFromBody(_descriptor.GetMethodDescription(options.ChannelMethod), options.Request.ContentType);

                if (requestParameters == hInfo.Parameters)
                {
                    _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                    return true;
                }
                else
                    return true;
            }
            else
            {
                _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                return true;
            }
        }

        private void _events_RemoveFromHeuristics(object sender, RemoveHeuristicsEventArgs e)
        {
            lock (_lock)
            {
                HeuristicsInfo expired = _cachedInfos.FirstOrDefault(x => x.Channel == e.Channel && x.ChannelMethod == e.ChannelMethod);

                _cachedInfos.Remove(expired);
            }

        }

        private void _events_AddToHeuristics(object sender, AddHeuristicsEventArgs e)
        {
            EnableCacheAttribute cache = e.MethodInfo.GetCustomAttribute<EnableCacheAttribute>();
            HeuristicsInfo addInfo = new HeuristicsInfo
            {
                Channel = e.Channel,
                ChannelMethod = e.MethodInfo,
                MethodResponse = e.MethodResponse,
                AddedTime = DateTime.Now,
                Parameters = e.Parameters,
                Duration = cache.Duration,
                DurationUnit = cache.Unit
            };

            lock (_lock)
            {
                _cachedInfos.Add(addInfo);
            }
        }

        [SuppressMessage("Nullable.Reference.Warning", "CS8632", Justification = "Needed for Timer constructor")]
        private void CollectExpiredCacheObject(object? state)
        {
            lock (_lock)
            {
                HeuristicsInfo[] expired = _cachedInfos.Where(x => x.Expired()).ToArray();
                foreach (HeuristicsInfo hInfo in expired)
                {
                    _cachedInfos.Remove(hInfo);

                }
            }
        }

    }
}
