using System;
using System.Net;

namespace Nuclear.Channels.Base
{
    public class ChannelRedirectionEventArgs : EventArgs
    {
        public string Url { get; set; }
        public HttpListenerResponse Response { get; set; }
    }
}
