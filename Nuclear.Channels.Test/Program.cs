using Nuclear.Channels.Base;
using System;

namespace Nuclear.Channels.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthMethods authMethods = new AuthMethods();
            IChannelHost host = ChannelHostBuilder.CreateHost();
            host.LoadAssemblies(AppDomain.CurrentDomain, null);
            host.AuthenticationOptions(authMethods.AuthenticateBasic);
            host.AuthenticationOptions(authMethods.AuthenticateToken);
            host.StartHosting(null);

            Console.ReadLine();
        }
    }
}
