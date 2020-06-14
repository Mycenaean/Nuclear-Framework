using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public class ChannelMethodGetRequest : ChannelRequest
    {
        public ChannelMethodGetRequest()
        {

        }

        public ChannelMethodGetRequest(string url, ChannelMethodParameters parameters, IChannelCredentials credentils) : base (url,parameters,credentils)
        {

        }
    }
}
