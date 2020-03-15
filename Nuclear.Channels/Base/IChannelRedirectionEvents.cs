using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Base
{
    internal interface IChannelRedirectionEvents
    {
        event EventHandler<ChannelRedirectionEventArgs> OnRedirectionInvoked;
        void ExecuteRedirection(string url, HttpListenerResponse response);
    }
}
