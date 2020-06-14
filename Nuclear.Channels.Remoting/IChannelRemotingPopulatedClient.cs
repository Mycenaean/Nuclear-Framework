using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public interface IChannelRemotingPopulatedClient
    {
        void Send();
        object GetResponse();
        TEntity GetResponse<TEntity>();
    }
}
