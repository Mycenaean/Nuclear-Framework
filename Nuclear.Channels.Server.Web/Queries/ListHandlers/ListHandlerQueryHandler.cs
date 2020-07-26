using Nuclear.Channels.Server.Web.Common;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Collections.Generic;

namespace Nuclear.Channels.Server.Web.Queries.ListHandlers
{
    [Export(typeof(IEventHandler<ListHandlersQuery, IReadOnlyCollection<HandlerInformation>>), ExportLifetime.Scoped)]
    internal class ListHandlerQueryHandler : CqrsHandlersBase, IEventHandler<ListHandlersQuery, IReadOnlyCollection<HandlerInformation>>
    {
        public IReadOnlyCollection<HandlerInformation> Handle(ListHandlersQuery request)
            => HandlerInfos;
    }
}