using Nuclear.Channels.Server.Web.Common;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace Nuclear.Channels.Server.Web.Queries.HandlerHistory
{
    [Export(typeof(IEventHandler<HandlerHistoryQuery, List<string>>), ExportLifetime.Scoped)]
    internal class HandlerHistoryQueryHandler : CqrsHandlersBase, IEventHandler<HandlerHistoryQuery, List<string>>
    {
        public List<string> Handle(HandlerHistoryQuery request)
        {
            var handler = HandlerInfos.FirstOrDefault(x => x.HandlerId == request.HandlerId);
            if (handler != null)
                return handler.History;
            else
                return null;
        }
    }
}
