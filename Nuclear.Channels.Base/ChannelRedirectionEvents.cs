using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Net;

namespace Nuclear.Channels.Base
{
    [Export(typeof(IChannelRedirectionEvents), ExportLifetime.Scoped)]
    internal class ChannelRedirectionEvents : IChannelRedirectionEvents
    {
        public event EventHandler<ChannelRedirectionEventArgs> OnRedirectionInvoked;

        public void ExecuteRedirection(string url, HttpListenerResponse response)
        {
            OnRedirectionInvoked?.Invoke(this, new ChannelRedirectionEventArgs { Url = url, Response = response });
        }

    }
}
