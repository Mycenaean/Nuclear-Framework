// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.Channels.Base.Decorators;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Server.Web.Commands.Plugins;
using Nuclear.Channels.Server.Web.Abstractions;
using Nuclear.Channels.Server.Web.Decorators;
using Nuclear.Channels.Server.Web.Queries.PluginsInitState;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Nuclear.Channels.Server
{
    /// <summary>
    /// Plugins Command and Control Center
    /// </summary>
    [Channel]
    [ProtectedHandler]
    [AuthorizeChannel(ChannelAuthenticationSchemes.Basic, "Role", "Elevated")]
    public class PluginsChannel : ChannelBase
    {
        [ImportedService]
        public IEventHandler<LoadPluginsCommand> PluginsHandler { get; set; }

        [ImportedService]
        public IEventHandler<PluginsInitStateQuery, PluginsInitState> PluginsStateHandler { get; set; }


        /// <summary>
        /// Endpoint that will copy the Dll`s containing channels from Blazor server to WebServer
        /// </summary>
        /// <param name="serverPluginsPath">Physical path to the Dll`s on the Blazor server</param>
        [ChannelMethod]
        public void InitPlugins(string serverPluginsPath)
        {
            var message = new ChannelMessage() { Success = true, Message = "Initialization of plugins started" };

            var loadPluginsCommand = new LoadPluginsCommand(serverPluginsPath);
            Task.Run(() => PluginsHandler.Handle(loadPluginsCommand));

            ChannelMessageWriter.Write(message, Context.Response);
        }

        /// <summary>
        /// Endpoint that will return status of InitPlugins method
        /// </summary>
        [ChannelMethod]
        public PluginsInitState PluginsInitStatus()
        {
            var initStateQuery = new PluginsInitStateQuery();

            //Its much cleaner to return object here than to construct ChannelMessage manually   
            return PluginsStateHandler.Handle(initStateQuery);
        }

    }
}
