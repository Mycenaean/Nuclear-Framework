using Nuclear.ExportLocator;
using System;
using System.Collections.Generic;

namespace Nuclear.Channels.Server.Web
{
    public static class ChannelsWebServerBuilder
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
            "Nuclear.Channels.Server.Web",
            "Nuclear.Channels.Server.Web.Api"
        };

        /// <summary>
        /// Creates IChannelWebServer instance
        /// </summary>
        /// <param name="serverAction">IChannelServer options</param>
        /// <param name="assemblies">Assemblies containing exported services</param>
        public static IChannelWebServer Build(Action<IChannelServer> serverAction)
        {
            var locator = ServiceLocatorBuilder.CreateServiceLocator(KnownExportAssemblies);
            var server = ChannelServerBuilder.CreateServer();
            serverAction(server);

            return new ChannelWebServer(locator, server);
        }
    }
}