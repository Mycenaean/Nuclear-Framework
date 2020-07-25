// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator;
using System;
using System.Collections.Generic;
using System.Text;
using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Server.Manager
{
    /// <summary>
    /// Server Manager Initiator
    /// </summary>
    public static class ChannelServerManagerBuilder
    {
        private static readonly List<string> KnownExportAssemblies = new List<string>()
        {
            "Nuclear.Channels",
            "Nuclear.Channels.Base",
            "Nuclear.Channels.Authentication",
            "Nuclear.Channels.Generators",
            "Nuclear.Channels.Heuristics",
            "Nuclear.Channels.InvokerServices",
            "Nuclear.Channels.Messaging",
            "Nuclear.Channels.Data",
            "Nuclear.Channels.Server.Manager",
            "Nuclear.Channels.Server"
        };

        /// <summary>
        /// Creates IChannelServerManager instance
        /// </summary>
        /// <param name="serverAction">IChannelServer options</param>
        public static IChannelServerManager Build(Action<IChannelServer> serverAction)
        {
            //Its important to initialise service locator before ChannelServer for the speed
            var locator = ServiceLocatorBuilder.CreateServiceLocator(KnownExportAssemblies);
            var server = ChannelServerBuilder.CreateServer();
            serverAction(server);

            return new ServerManager(locator, server);
        }

        /// <summary>
        /// Creates IChannelServerManager instance
        /// </summary>
        /// <param name="serverAction">IChannelServer options</param>
        /// <param name="exportAssemblies">Additional assemblies containing exported services</param>
        /// <returns></returns>
        public static IChannelServerManager Build(Action<IChannelServer> serverAction, List<string> exportAssemblies)
        {
            //Its important to initialise IServiceLocator before ChannelServer for the speed
            if (exportAssemblies != null && exportAssemblies.Count > 0)
                KnownExportAssemblies.AddRange(exportAssemblies);
            
            var locator = ServiceLocatorBuilder.CreateServiceLocator(KnownExportAssemblies);
            var server = ChannelServerBuilder.CreateServer();
            serverAction(server);

            return new ServerManager(locator, server);
        }
    }
}