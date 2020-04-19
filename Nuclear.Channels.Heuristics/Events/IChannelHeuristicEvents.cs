using Nuclear.Channels.Heuristics.EventArguments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Heuristics.Events
{
    public interface IChannelHeuristicEvents
    {
        event EventHandler<AddHeuristicsEventArgs> AddToHeuristics;
        event EventHandler<RemoveHeuristicsEventArgs> RemoveFromHeuristics;

        void OnAddToHeuristics(AddHeuristicsEventArgs args);
        void OnRemoveFromHeuristics();
    }
}

