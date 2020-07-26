using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Web.Common;
using Nuclear.ExportLocator.Services;
using System.Threading;

namespace Nuclear.Channels.Server.Web
{
    internal class ChannelWebServer : IChannelWebServer
    {
        private readonly IServiceLocator _services;
        private readonly IChannelServer _server;

        public ChannelWebServer(IServiceLocator services, IChannelServer server)
        {
            _services = services;
            _server = server;
        }

        public void Start()
        {
            _server.StartHosting(null);
            Thread.Sleep(4000);
            var methodHandlers = _services.Get<IChannelMethodHandlerCollection>().AsList();
            var initiatedHandlers = _services.Get<IInitiatedHandlersCollection>();

            foreach (var handler in methodHandlers)
            {
                initiatedHandlers.AddHandler(handler.HandlerId, handler.Url, handler.StateName);
            }
        }
    }
}