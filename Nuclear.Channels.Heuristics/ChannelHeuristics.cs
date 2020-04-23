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

        public ChannelHeuristics()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _descriptor = _services.Get<IChannelMethodDescriptor>();
            _msgService = _services.Get<IChannelMessageService>();
            _events = _services.Get<IChannelHeuristicEvents>();

            _events.AddToHeuristics += _events_AddToHeuristics;
            _events.RemoveFromHeuristics += _events_RemoveFromHeuristics;
            _cachedInfos = new List<HeuristicsInfo>();

        }

        public bool IsMethodCached(Type channel, MethodInfo channelMethod, out HeuristicsInfo hInfo)
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

        public CacheExecutionResult Execute(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo)
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

        public HeuristicsInfo[] GetExpiredCache()
        {
            return _cachedInfos.Where(x => x.Expired()).ToArray();
        }

        private CacheExecutionResult TryInvokeGetMethod(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo)
        {
            CacheExecutionResult result = new CacheExecutionResult();
            if (options.Request.QueryString.AllKeys.Length == 0)
            {
                _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                result.Executed = true;
                return result;
            }
            else
            {
                ChannelMethodDeserializerFactory dsrFactory = new ChannelMethodDeserializerFactory(options.Request.QueryString);
               List<object> requestParameters = dsrFactory.DeserializeFromQueryParameters(_descriptor.GetMethodDescription(options.ChannelMethod));

                if (ParameteresMatch(hInfo.Parameters, requestParameters))
                {
                    _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                    result.Executed = true;
                    return result;
                }
                else
                {
                    result.Executed = false;
                    result.DataProcessed = true;
                    result.Data.HasData = true;
                    result.Data.Parameters = requestParameters;
                    return result;
                }
            }

        }

        private bool ParameteresMatch(List<object> cached, List<object> current)
        {
            IEnumerable<object> notInCache = cached.Except(current);
            IEnumerable<object> notInCurrent = current.Except(cached);

            return (notInCache.Count() + notInCurrent.Count()) == 0;
        }

        private CacheExecutionResult TryInvokePostMethod(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo)
        {
            CacheExecutionResult result = new CacheExecutionResult();
            if (options.Request.HasEntityBody)
            {
                ChannelMethodDeserializerFactory dsrFactory = new ChannelMethodDeserializerFactory(options.Request.InputStream);
                List<object> requestParameters = dsrFactory.DeserializeFromBody(_descriptor.GetMethodDescription(options.ChannelMethod), options.Request.ContentType);

                if (ParameteresMatch(hInfo.Parameters, requestParameters))
                {
                    _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                    result.Executed = true;
                }
                else 
                {
                    result.Executed = false;
                    result.DataProcessed = true;
                    result.Data.HasData = true;
                    result.Data.Parameters = requestParameters;
                }
            }
            else
            {
                _msgService.WriteHttpResponse(hInfo.MethodResponse, options.Response);
                result.Executed = true;
            }

            return result;
        }

        private void _events_RemoveFromHeuristics(object sender, RemoveHeuristicsEventArgs e)
        {
            if (e.SingleForRemoval != null)
                _cachedInfos.Remove(e.SingleForRemoval);
            else
            {
                foreach (HeuristicsInfo expired in e.CollectionForRemoval)
                {
                    _cachedInfos.Remove(expired);
                }
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

            _cachedInfos.Add(addInfo);
        }


    }
}
