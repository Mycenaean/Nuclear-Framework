using System.Net;

namespace Nuclear.Channels.Remoting.Services
{
    public interface IChannelRemotingService
    {
        string InvokeChannel(string URL, string HttpMethod, string username = null, string password = null);
        string InvokeChannel(string channel, string channelmethod, string baseURL);
        string InvokeChannel(string channel, string channelmethod, string baseURL, object inputBody);
        string Invoke(HttpWebRequest request);
    }

}
