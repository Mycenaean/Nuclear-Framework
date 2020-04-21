// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    /// <summary>
    /// Service which writes cached response to the client
    /// </summary>
    public interface IChannelHeuristics
    {
        bool IsMethodCached(Type channel, MethodInfo channelMethod, out HeuristicsInfo hInfo);
        bool Execute(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo);
    }
}
