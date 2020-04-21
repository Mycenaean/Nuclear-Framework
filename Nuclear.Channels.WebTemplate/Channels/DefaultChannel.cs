using Nuclear.Channels.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nuclear.Channels.WebTemplate.Channels
{
    [Channel]
    public class DefaultChannel : ChannelBase
    {        
        [ChannelMethod]
        public string Hello()
        {
            return "Hello from ChannelMethod";
        }
    }
}
