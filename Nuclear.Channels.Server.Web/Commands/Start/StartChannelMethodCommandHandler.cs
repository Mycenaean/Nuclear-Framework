﻿using Nuclear.Channels.Server.Web.Common;
using Nuclear.Channels.Server.Web.Exceptions;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nuclear.Channels.Server.Web.Commands.Start
{
    [Export(typeof(IEventHandler<StartChannelMethodCommand>), ExportLifetime.Scoped)]
    internal class StartChannelMethodCommandHandler : CqrsHandlersBase, IEventHandler<StartChannelMethodCommand>
    {
        public void Handle(StartChannelMethodCommand request)
        {
            var handler = HandlerInfos.FirstOrDefault(x => x.HandlerId == request.HandlerId);
            if (handler == null) throw new HandlerNotFoundException();
            if (handler.State.Equals("started", System.StringComparison.OrdinalIgnoreCase))
                return;

            var rawHandler = RawHandlers.FirstOrDefault(x => x.HandlerId == request.HandlerId);
            Task.Run(() => { rawHandler.Start(); });
            Thread.Sleep(1000);

            HandlerInfoCollection.UpdateHandlerState(GetType().Name, request.HandlerId, "Running");
        }
    }
}
