using Nuclear.Channels.Messaging.Services.ChannelMessage;
using Nuclear.Channels.Messaging.Services.Output;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;

namespace Nuclear.Channels.Messaging
{
    /// <summary>
    /// Event Handler for ChannelMethodOutputService
    /// </summary>
    [Export(typeof(IChannelMessageWriter), ExportLifetime.Transient)]
    public class ChannelMessageWriter : IChannelMessageWriter
    {
        public event EventHandler<ChannelMethodEventArgs> SendChannelMessage;
        private ChannelMethodEventArgs args;

        public void Send(IChannelMessage message)
        {
            args = new ChannelMethodEventArgs() { ChannelMessage = message };
            SendChannelMessage?.Invoke(this, args);
        }

    }
}
