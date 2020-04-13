using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public class ChannelBasicCredentials : IChannelCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
