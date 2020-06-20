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
        private IServiceLocator _services;
        private IChannelHandlerCollection _channelHandlers;
        private IChannelMethodHandlerCollection _methodHandlers;

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
                ChannelHandler channelHandler = _channelHandlers.GetHandler(context.HandlerId);
                switch (context.CommandType)
                {
                    case ServerCommandType.Read:
                        return new ReadStateCommand(channelHandler);
                    case ServerCommandType.Restart:
                        return new RestartCommand(channelHandler);
                    case ServerCommandType.Start:
                        return new StartCommand(channelHandler);
                    case ServerCommandType.Stop:
                        return new StopCommand(channelHandler);
                    default:
                        return null;
                }
            }
            else
            {
                ChannelMethodHandler methodHandler = _methodHandlers.GetHandler(context.HandlerId);
                switch (context.CommandType)
                {
                    case ServerCommandType.Read:
                        return new ReadStateCommand(methodHandler);
                    case ServerCommandType.Restart:
                        return new RestartCommand(methodHandler);
                    case ServerCommandType.Start:
                        return new StartCommand(methodHandler);
                    case ServerCommandType.Stop:
                        return new StopCommand(methodHandler);
                    default:
                        return null;
                }
            }
        }

        public IServerCommand GetCoreCommand(CoreCommand coreCommand)
        {
            switch (coreCommand.CommandName)
            {
                case "InitPlugins":
                    return new InitPluginsCommand();
                case "InitServer":
                    {
                        IChannelServer server = coreCommand.Services.FirstOrDefault() as IChannelServer;

                        return new InitServerCommand(server);
                    }
                case "ServerThread":
                    {
                        InitServerCommand initServerCommand = coreCommand.Services[0] as InitServerCommand;
                        IServiceLocator services = coreCommand.Services[1] as IServiceLocator;
                        IConsoleWriter writer = coreCommand.Services[2] as IConsoleWriter;

                        return new ServerThreadCommand(services, writer, initServerCommand);
                    }
                default:
                    return null;
            }
        }
    }
}
