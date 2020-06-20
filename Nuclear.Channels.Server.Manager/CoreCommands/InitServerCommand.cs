using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Manager.Commands;
using Nuclear.Channels.Server.Manager.Console;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager.CoreCommands
{
    public class InitServerCommand : IServerCommand
    {
        private IChannelServer _server;

        public InitServerCommand(IChannelServer server)
        {
            _server = server;
        }

        public void Execute()
        {
            _server.StartHosting(null);
        }
    }
}
