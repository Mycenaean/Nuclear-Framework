using Nuclear.Channels.Server.Web.Common;
using Nuclear.Channels.Server.Web.Exceptions;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.Channels.Server.Web.Abstractions;
using System;
using System.Linq;

namespace Nuclear.Channels.Server.Web.Commands.Stop
{
    [Export(typeof(IEventHandler<StopChannelMethodCommand>), ExportLifetime.Scoped)]
    internal class StopChannelMethodCommandHandler : EventHandlerBase, IEventHandler<StopChannelMethodCommand>
    {
        public void Handle(StopChannelMethodCommand request)
        {
            var handlerInfo = HandlerInfos.FirstOrDefault(x => x.HandlerId == request.HandlerId);
            if (handlerInfo == null) throw new HandlerNotFoundException();

            if (handlerInfo.State.Equals("stopped", StringComparison.OrdinalIgnoreCase))
                return;

            var handler = RawHandlers.FirstOrDefault(x => x.HandlerId == request.HandlerId);

            handler.Stop();
            HandlerInfoCollection.UpdateHandlerState(GetType().Name, request.HandlerId, "Stopped");
        }
    }
}
