using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Base
{
    public class ChannelRedirectionEventArgs : EventArgs
    {
        public string Url { get; set; }
        public HttpListenerResponse Response { get; set; }
    }
}
