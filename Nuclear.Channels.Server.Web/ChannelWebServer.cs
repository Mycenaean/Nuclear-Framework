using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Web.Common;
using Nuclear.Channels.Server.Web.Logging;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Threading;

namespace Nuclear.Channels.Server.Web
{
    [Export(typeof(IChannelWebServer), ExportLifetime.Singleton)]
    internal class ChannelWebServer : IChannelWebServer
    {
        private readonly IServiceLocator _services;
        private readonly IChannelServer _server;
        private IChannelServer _serverCopy;

        public IChannelServer Server { get; set; }
        public IChannelServer ServerCopy { get; private set; }

        public ChannelWebServer(IServiceLocator services, IChannelServer server)
        {
            _services = services;
            _server = server;
            _serverCopy = _server;
        }

        public void Start()
        {
            _server.StartHosting(null);
            Thread.Sleep(4000);
            ServerLoggerFactory.Init();

            var methodHandlers = _services.Get<IChannelMethodHandlerCollection>().AsList();
            var initiatedHandlers = _services.Get<IInitiatedHandlersCollection>();

            methodHandlers.ForEach(handler =>
            {
                var channelName = handler.Url.Split('/')[4];
                var isProtected = "ServerChannel".Equals(channelName, StringComparison.OrdinalIgnoreCase)
                || "PluginsChannel".Equals(channelName, StringComparison.OrdinalIgnoreCase);

                initiatedHandlers.AddHandler(handler.HandlerId, handler.Url, handler.StateName, isProtected);
            });
        }
    }
}