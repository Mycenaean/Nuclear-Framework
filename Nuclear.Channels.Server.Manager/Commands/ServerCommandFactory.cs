using Nuclear.Channels.Handlers;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Server.Manager.Commands
{
    [Export(typeof(IServerCommandFactory), ExportLifetime.Singleton)]
    internal class ServerCommandFactory : IServerCommandFactory
    {
        private IServiceLocator _services;
        private IChannelHandlerCollection _channelHandlers;
        private IChannelMethodHandlerCollection _methodHandlers;

        public ServerCommandFactory()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _channelHandlers = _services.Get<IChannelHandlerCollection>();
            _methodHandlers = _services.Get<IChannelMethodHandlerCollection>();
        }


        public IServerCommand GetCommand(ServerCommandContext context)
        {
            if(context.CommandTarget == ServerCommandTarget.Channel)
            {
                ChannelHandler channelHandler = _channelHandlers.GetHandler(context.HandlerId);
                switch(context.CommandType)
                {
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
                switch(context.CommandType)
                {
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
    }
}
