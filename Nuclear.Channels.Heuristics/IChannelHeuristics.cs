using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    public interface IChannelHeuristics
    {
        bool IsMethodCached(Type channel, MethodInfo channelMethod, out HeuristicsInfo hInfo);
        bool Execute(ChannelMethodHeuristicOptions options, HeuristicsInfo hInfo);
    }
}
