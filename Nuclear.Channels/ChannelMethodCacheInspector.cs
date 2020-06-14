// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Heuristics;
using Nuclear.Channels.Heuristics.Contexts;
using Nuclear.Channels.Messaging;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Nuclear.Channels
{
    /// <summary>
    /// Internal cache inspector
    /// </summary>
    internal class ChannelMethodCacheInspector
    {
        private readonly IChannelMessageService _msgService;
        private readonly IChannelHeuristics _heuristics;

        public ChannelMethodCacheInspector(IChannelMessageService msgService, IChannelHeuristics heuristics)
        {
            _msgService = msgService;
            _heuristics = heuristics;
        }

        internal bool IsCached(MethodInfo methodInfo)
        {
            EnableCacheAttribute cache = methodInfo.GetCustomAttribute<EnableCacheAttribute>();
            return cache != null;
        }

        internal static void CheckCacheValidity(MethodInfo methodInfo)
        {
            EnableCacheAttribute cache = methodInfo.GetCustomAttribute<EnableCacheAttribute>();
            if (methodInfo.ReturnType == typeof(void) && cache != null)
                throw new InvalidChannelMethodTargetException("EnableCache can not be applied to a method with return type void");
        }

        internal CacheExecutionResult ExecuteIfCached(Type channel, MethodInfo method, HttpListenerRequest request, HttpListenerResponse response, IChannelHeuristicContext heurContext)
        {
            CacheExecutionResult result = new CacheExecutionResult();
            result.Executed = false;
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = IsCached(method);
            }
            catch (InvalidChannelMethodTargetException ex)
            {
                StreamWriter writer = new StreamWriter(response.OutputStream);
                _msgService.ExceptionHandler(writer, ex, response);
                writer.Close();
                result.Executed = true;
            }

            if (isCacheEnabled)
            {
                HeuristicsInfo hInfo = new HeuristicsInfo();
                bool isCached = _heuristics.IsMethodCached(channel, method, out hInfo);
                if (isCached)
                {
                    ChannelMethodHeuristicOptions hOptions = new ChannelMethodHeuristicOptions
                    {
                        Channel = channel,
                        ChannelMethod = method,
                        Request = request,
                        Response = response
                    };
                    return _heuristics.Execute(hOptions, hInfo);
                }
                else
                {
                    heurContext.ExpectsAdding = true;
                    heurContext.Channel = channel;
                    heurContext.MethodInfo = method;
                    result.Executed = false;
                    result.DataProcessed = false;
                }
            }

            return result;
        }

    }
}
