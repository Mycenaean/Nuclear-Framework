// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Handlers;
using Nuclear.ExportLocator;
using System.Collections.Generic;

namespace Nuclear.Channels.Server.Web.Common
{
    public abstract class EventHandlerBase
    {
        protected IInitiatedHandlersCollection HandlerInfoCollection { get; }
        protected IReadOnlyCollection<HandlerInformation> HandlerInfos { get; }
        protected List<ChannelMethodHandler> RawHandlers { get; }

        public EventHandlerBase()
        {
            RawHandlers = ServiceFactory.GetExportedService<IChannelMethodHandlerCollection>().AsList();
            HandlerInfoCollection = ServiceFactory.GetExportedService<IInitiatedHandlersCollection>();
            HandlerInfos = HandlerInfoCollection.Handlers;
        }
    }
}
