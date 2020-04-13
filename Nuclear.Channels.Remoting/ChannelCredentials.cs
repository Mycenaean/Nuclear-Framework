using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public class ChannelCredentials
    {
        IChannelCredentials GetCredentials(ChannelCredentialsType type)
        {
            if (type == ChannelCredentialsType.Basic)
                return new ChannelBasicCredentials();
            else
                return new ChannelTokenCredentials();
        }
    }
}
