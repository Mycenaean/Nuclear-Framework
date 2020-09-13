// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.Common;
using Nuclear.ExportLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Nuclear.Channels.Server.Web.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Channels Web Server API");
            Console.WriteLine("Version 1.0");

            var server = ChannelsWebServerBuilder.Build(options =>
            {
                options.RegisterChannels(new List<string> { "Nuclear.Channels.Server.Web" });
                options.IsServerManaged(true);
            });

            server.Start();

            var handlerInfos = ServiceFactory.GetExportedService<IInitiatedHandlersCollection>()
                .Handlers.ToList();

            Thread.Sleep(1000);
            handlerInfos.ForEach(x => Console.WriteLine($"{x.HandlerId} {x.Url} {x.State}"));

            Console.ReadLine();
        }
    }
}
