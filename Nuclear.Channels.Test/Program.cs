using System;
using Nuclear.Channels.Authentication.Extensions;

namespace Nuclear.Channels.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthMethods authMethods = new AuthMethods();
            IChannelServer host = ChannelServerBuilder.CreateServer();
            host.LoadAssemblies(AppDomain.CurrentDomain, null);
            host.AddTokenAuthentication(authMethods.AuthenticateToken);
            host.StartHosting(null);

            Console.ReadLine();
        }
    }
}
