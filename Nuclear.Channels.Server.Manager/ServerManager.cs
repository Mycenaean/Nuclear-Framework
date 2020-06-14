using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Server.Manager
{
    [Export(typeof(IChannelServerManager), ExportLifetime.Singleton)]
    internal class ServerManager : IChannelServerManager
    {
        private IServiceLocator _services;

        public IChannelServer Server { get; }
        public IServerInstructionCollection Instructions { get; }

        public ServerManager()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();

            Server = ChannelServerBuilder.CreateServer();
            Instructions = _services.Get<IServerInstructionCollection>();
        }
    }

}