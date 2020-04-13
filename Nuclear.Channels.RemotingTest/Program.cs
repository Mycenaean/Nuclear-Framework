using Nuclear.Channels.Remoting;
using System;

namespace Nuclear.Channels.RemotingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IChannelRemotingClient client = new ChannelRemotingClient();
            ChannelMethodPostRequest postRequest = new ChannelMethodPostRequest();
            postRequest.Parameters = new ChannelMethodParameters(RequestContentType.XML);
            postRequest.Parameters.AddParameter("name", "Nikola");
            postRequest.Url = "http://localhost:4200/channels/TestChannel/PostParams/";
            string response = client.GetResponse<string>(postRequest);
            Console.WriteLine(response);
            Console.ReadLine();
        }
    }
}
