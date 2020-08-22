// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nuclear.Channels.Server.Web.AspNetCoreExtensions
{
    public class ChannelEndpointProvider : IChannelEndpointProvider
    {
        private string[] _pluginsEndpoints;
        private string[] _serverEndpoints;

        public ChannelEndpointProvider()
        {
            var endpoints = Init();
            _serverEndpoints = endpoints.Key;
            _pluginsEndpoints = endpoints.Value;
        }

        public string GetEndpointUrl(string baseUrl, string methodName)
        {
            string endpoint = string.Empty;
            Func<string, bool> methodCompare = x => x.Equals(methodName, StringComparison.OrdinalIgnoreCase);

            if (baseUrl.Contains("ServerChannel"))
            {
                
                endpoint = _serverEndpoints.FirstOrDefault(methodCompare);
            }
            else if (baseUrl.Contains("PluginsChannel"))
            {
                endpoint = _pluginsEndpoints.FirstOrDefault(methodCompare);
            }

            if (String.IsNullOrEmpty(endpoint))
                throw new ArgumentException(nameof(methodName));

            return $"{baseUrl}{endpoint}";

        }

        private KeyValuePair<string[], string[]> Init()
        {
            var serverChannel = new ServerChannelEndpoints();
            var pluginsChannel = new PluginsChannelEndpoints();

            var sFields = typeof(ServerChannelEndpoints).GetFields();
            var pFields = typeof(PluginsChannelEndpoints).GetFields();

            var sfieldValues = new string[sFields.Length];
            var pfieldValues = new string[pFields.Length];

            for (int i = 0; i < sFields.Length; i++)
            {
                sfieldValues[i] = sFields[i].GetValue(serverChannel) as string;
            }

            for (int i = 0; i < pFields.Length; i++)
            {
                pfieldValues[i] = pFields[i].GetValue(pluginsChannel) as string;
            }

            return new KeyValuePair<string[], string[]>(sfieldValues, pfieldValues);
        }
    }
}
