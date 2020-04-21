// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

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
        void OnRemoveFromHeuristics(RemoveHeuristicsEventArgs args);
    }
}

