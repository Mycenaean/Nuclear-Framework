using Nuclear.Channels.Messaging.Services.ChannelMessage;
using System;

namespace Nuclear.Channels.Messaging.Services.Output
{
    public interface IChannelMessageWriter
    {
        event EventHandler<ChannelMethodEventArgs> SendChannelMessage;
        void Send(IChannelMessage message);
    }
}
