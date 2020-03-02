using Nuclear.Channels.Base;
using System;

namespace Nuclear.Channels.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthMethods authMethods = new AuthMethods();
            IChannelServer host = ChannelServerBuilder.CreateServer();
            host.LoadAssemblies(AppDomain.CurrentDomain, null);
            host.AuthenticationOptions(authMethods.AuthenticateBasic);
            host.AuthenticationOptions(authMethods.AuthenticateToken);
            host.StartHosting("http://nmilinkovic:4200");

            Console.ReadLine();
        }
    }
}
