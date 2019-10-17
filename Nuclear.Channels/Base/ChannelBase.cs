using Nuclear.Channels.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Messaging.Services.Output;
using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Base
{
    /// <summary>
    /// Base abstract WebChannel Helper class
    /// </summary>
    public class ChannelBase : IChannel
    {
        /// <summary>
        /// ServiceLocator
        /// </summary>
        public IServiceLocator Services => ServiceLocator.GetInstance;

        /// <summary>
        /// Service that will write ChannelMessage to the client
        /// </summary>
        public IChannelMessageWriter MessageWriter => new ChannelMessageWriter();

    }
}
