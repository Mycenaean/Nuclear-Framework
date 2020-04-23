using Nuclear.Channels.Base;
using System;
using System.Collections.Generic;
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
