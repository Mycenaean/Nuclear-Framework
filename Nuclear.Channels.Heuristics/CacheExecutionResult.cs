// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    public class CacheExecutionResult
    {
        public bool Executed { get; set; }
        public bool DataProcessed { get; set; }
        public ChannelMethodRequestData Data { get; set; }

        public CacheExecutionResult()
        {
            Data = new ChannelMethodRequestData();
        }

    }


}
