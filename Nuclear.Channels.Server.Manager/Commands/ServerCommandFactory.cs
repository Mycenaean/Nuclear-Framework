// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Manager.Console;
using Nuclear.Channels.Server.Manager.CoreCommands;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System.Diagnostics;
using System.Linq;

namespace Nuclear.Channels.Server.Manager.Commands
{
    [Export(typeof(IServerCommandFactory), ExportLifetime.Singleton)]
    internal class ServerCommandFactory : IServerCommandFactory
    {
        private readonly IServiceLocator _services;
        private readonly IChannelHandlerCollection _channelHandlers;
        private readonly IChannelMethodHandlerCollection _methodHandlers;

        [DebuggerStepThrough]
        public ServerCommandFactory()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _channelHandlers = _services.Get<IChannelHandlerCollection>();
            _methodHandlers = _services.Get<IChannelMethodHandlerCollection>();
        }


        public IServerCommand GetCommand(ServerCommandContext context)
        {
            if (context.CommandTarget == ServerCommandTarget.Channel)
            {
                var channelHandler = _channelHandlers.GetHandler(context.HandlerId);
                return context.CommandType switch
                {
                    ServerCommandType.Read => new ReadStateCommand(channelHandler),
                    ServerCommandType.Restart => new RestartCommand(channelHandler),
                    ServerCommandType.Start => new StartCommand(channelHandler),
                    ServerCommandType.Stop => new StopCommand(channelHandler),
                    _ => null
                };
            }
            else
            {
                var methodHandler = _methodHandlers.GetHandler(context.HandlerId);
                return context.CommandType switch
                {
                    ServerCommandType.Read => new ReadStateCommand(methodHandler),
                    ServerCommandType.Restart => new RestartCommand(methodHandler),
                    ServerCommandType.Start => new StartCommand(methodHandler),
                    ServerCommandType.Stop => new StopCommand(methodHandler),
                    _ => null
                };
            }
        }

        public IServerCommand GetCoreCommand(CoreCommand coreCommand)
        {
            switch (coreCommand.CommandName)
            {
                case "InitServer":
                    {
                        var server = coreCommand.Services.FirstOrDefault() as IChannelServer;

                        return new InitServerCommand(server);
                    }
                case "ServerThread":
                    {
                        var initServerCommand = coreCommand.Services[0] as InitServerCommand;
                        var services = coreCommand.Services[1] as IServiceLocator;
                        var writer = coreCommand.Services[2] as IConsoleWriter;

                        return new ServerThreadCommand(services, writer, initServerCommand);
                    }
                default:
                    return null;
            }
        }
    }
}
