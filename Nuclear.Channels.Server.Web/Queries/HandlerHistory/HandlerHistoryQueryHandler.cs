// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.Common;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.Channels.Server.Web.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Nuclear.Channels.Server.Web.Queries.HandlerHistory
{
    [Export(typeof(IEventHandler<HandlerHistoryQuery, List<string>>), ExportLifetime.Scoped)]
    internal class HandlerHistoryQueryHandler : EventHandlerBase, IEventHandler<HandlerHistoryQuery, List<string>>
    {
        public List<string> Handle(HandlerHistoryQuery request)
        {
            var handler = HandlerInfos.FirstOrDefault(x => x.HandlerId == request.HandlerId);
            return handler != null ? handler.History : null;
        }
    }
}
