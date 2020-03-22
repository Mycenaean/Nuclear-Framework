using System;
using System.Net;

namespace Nuclear.Channels.Base
{
    public interface IChannelRedirectionEvents
    {
        event EventHandler<ChannelRedirectionEventArgs> OnRedirectionInvoked;
        void ExecuteRedirection(string url, HttpListenerResponse response);
    }
}
