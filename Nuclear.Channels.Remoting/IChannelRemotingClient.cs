using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public interface IChannelRemotingClient : IChannelRemotingPopulatedClient
    {
        void Send(ChannelRequest request);
        object GetResponse(ChannelRequest request);
        TEntity GetResponse<TEntity>(ChannelRequest request);
    }
}
