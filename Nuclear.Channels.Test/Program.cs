﻿using System;
using System.Collections.Generic;
using Nuclear.Channels.Authentication.Extensions;
using Nuclear.Channels.Heuristics.CacheCleaner;

namespace Nuclear.Channels.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthMethods authMethods = new AuthMethods();
            IChannelServer host = ChannelServerBuilder.CreateServer();
            //host.LoadAssemblies(AppDomain.CurrentDomain, null);
            List<string> asm = new List<string>
            {
                "Nuclear.Channels.Test"
            };

            host.RegisterChannels(asm);
            host.AddTokenAuthentication(authMethods.AuthenticateToken);
            //host.ConfigureCacheCleaner(TimeSpan.FromSeconds(30));
            host.StartHosting(null);

            Console.ReadLine();
        }
    }
}
