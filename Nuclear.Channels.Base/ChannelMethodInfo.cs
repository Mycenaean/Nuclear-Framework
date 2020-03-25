using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Base
{
    public class ChannelMethodInfo
    {
        public List<ChannelMethodParameter> Parameters { get; }

        public ChannelMethodInfo()
        {
            Parameters = new List<ChannelMethodParameter>();
        }

        public void AddParameter(string name, Type type)
        {
            Parameters.Add(new ChannelMethodParameter { Name = name, Type = type });
        }
    }
}

