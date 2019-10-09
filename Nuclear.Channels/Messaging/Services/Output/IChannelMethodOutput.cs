using Nuclear.Channels.Messaging.Services.ChannelMessage;
using System;

namespace Nuclear.Channels.Messaging.Services.Output
{
    public interface IChannelMethodOutput
    {
        event EventHandler<ChannelMethodEventArgs> WriteEventHandler;
        void Send(IChannelMessage message);
    }
}
