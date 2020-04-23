using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Base
{
    public class ChannelMethodRequestData
    {
        public bool HasData { get; set; }
        public List<object> Parameters { get; set; }
    }
}
