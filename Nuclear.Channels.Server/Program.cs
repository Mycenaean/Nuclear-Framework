// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Manager;
using System;

namespace Nuclear.Channels.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverManager = ChannelServerManagerBuilder.Build(server => 
            {
                server.LoadAssemblies(AppDomain.CurrentDomain, null);
                server.IsServerManaged(true);
            });

            serverManager.Start();            
        }
    }
}

