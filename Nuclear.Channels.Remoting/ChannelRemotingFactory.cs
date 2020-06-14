using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public class ChannelRemotingFactory
    {

        public static IChannelRemotingClient GetClient()
        {
            return new ChannelRemotingClient();
        }

        public static IChannelRemotingPopulatedClient GetClient(ChannelRequest request)
        {
            return new ChannelRemotingClient(request);
        }

        public static IChannelRemotingPopulatedClient GetClient(string url, ChannelMethodParameters parameters, IChannelCredentials credentials , string httpMethod)
        {
            ChannelRequest request = null;

            if (httpMethod.Equals("get", StringComparison.OrdinalIgnoreCase))
                request = new ChannelMethodGetRequest(url, parameters, credentials);
            else if (httpMethod.Equals("post", StringComparison.OrdinalIgnoreCase))
                request = new ChannelMethodPostRequest(url, parameters, credentials);
            else
                throw new ArgumentException("HttpMethod can be either GET or POST");
            
            return new ChannelRemotingClient(request);
            
        }
    }
}
