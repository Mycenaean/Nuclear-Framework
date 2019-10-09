using Nuclear.Channels.Messaging.Services.ChannelMessage;
using System;

namespace Nuclear.Channels.Messaging
{
    public class ChannelMethodEventArgs : EventArgs
    {
        public IChannelMessage ChannelMessage { get; set; }
    }
}
