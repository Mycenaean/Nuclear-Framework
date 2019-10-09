using Nuclear.Channels.Contracts;
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

    }
}
