using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public class ChannelMethodPostRequest : ChannelRequest
    {
        public ChannelMethodPostRequest()
        {

        }

        public ChannelMethodPostRequest(string url, ChannelMethodParameters parameters, IChannelCredentials credentils) : base(url, parameters, credentils)
        {

        }
    }
}
