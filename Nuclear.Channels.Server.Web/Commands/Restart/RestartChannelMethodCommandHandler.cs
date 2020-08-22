// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.Common;
using Nuclear.Channels.Server.Web.Exceptions;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.Channels.Server.Web.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nuclear.Channels.Server.Web.Commands.Restart
{
    [Export(typeof(IEventHandler<RestartChannelMethodCommand>), ExportLifetime.Scoped)]
    internal class RestartChannelMethodCommandHandler : EventHandlerBase, IEventHandler<RestartChannelMethodCommand>
    {
        public void Handle(RestartChannelMethodCommand request)
        {
            var handler = HandlerInfos.FirstOrDefault(x => x.HandlerId == request.HandlerId);
            if (handler == null) throw new HandlerNotFoundException();

            var rawHandler = RawHandlers.FirstOrDefault(x => x.HandlerId == request.HandlerId);
            Task.Run(() => { rawHandler.Restart(); });
            Thread.Sleep(1000);

            HandlerInfoCollection.UpdateHandlerState(GetType().Name, request.HandlerId, "Restarted");
        }
    }
}
