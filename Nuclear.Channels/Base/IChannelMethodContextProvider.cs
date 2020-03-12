using Nuclear.Channels.Contracts;

namespace Nuclear.Channels.Base
{
    internal interface IChannelMethodContextProvider
    {
        void SetChannelMethodContext(IChannelEndpoint endpoint, IChannelMethodContext context);
        void DestroyChannelMethodContext(IChannelEndpoint endpoint);
        IChannelMethodContext GetChannelMethodContext(IChannelEndpoint endpoint);
    }
}