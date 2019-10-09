using Nuclear.Channels.Base;
using System;

namespace Nuclear.Channels.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IChannelHost host = ChannelHost.GetHost;
            host.LoadAssemblies(AppDomain.CurrentDomain, null);
            host.StartHosting(null);

            Console.ReadLine();
        }
    }
}
