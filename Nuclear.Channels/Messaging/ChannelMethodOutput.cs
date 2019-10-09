using Nuclear.Channels.Messaging.Services.ChannelMessage;
using Nuclear.Channels.Messaging.Services.Output;
using Nuclear.ExportLocator.Decorators;
using System;

namespace Nuclear.Channels.Messaging
{
    /// <summary>
    /// Event Handler for ChannelMethodOutputService
    /// </summary>
    [Export(typeof(IChannelMethodOutput))]
    public class ChannelMethodOutput : IChannelMethodOutput
    {
        public event EventHandler<ChannelMethodEventArgs> WriteEventHandler;
        private ChannelMethodEventArgs args;

        public void Send(IChannelMessage message)
        {
            args = new ChannelMethodEventArgs();
            args.ChannelMessage = message;
            WriteEventHandler?.Invoke(this, args);
        }

    }
}
