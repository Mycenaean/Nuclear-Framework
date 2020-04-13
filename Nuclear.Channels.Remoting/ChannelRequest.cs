using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public abstract class ChannelRequest
    {
        public string Url { get; set; }
        public ChannelMethodParameters Parameters { get; set; }
        public IChannelCredentials Credentials { get; set; }

        public ChannelRequest()
        {
            
        }

        public ChannelRequest(RequestContentType contentType)
        {
            if (Parameters == null)
                Parameters = new ChannelMethodParameters(contentType);
        }

        public ChannelRequest(string url, ChannelMethodParameters parameters, IChannelCredentials credentils)
        {
            Url = url;
            Parameters = parameters;
            Credentials = credentils;
        }
    }
}
