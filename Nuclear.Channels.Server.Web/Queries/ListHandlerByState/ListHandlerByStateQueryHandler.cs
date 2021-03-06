﻿// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.Common;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.Channels.Server.Web.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nuclear.Channels.Server.Web.Queries.ListHandlerByState
{
    [Export(typeof(IEventHandler<ListHandlerByStateQuery, IEnumerable<HandlerInformation>>), ExportLifetime.Scoped)]
    internal class ListHandlerByStateQueryHandler : EventHandlerBase, IEventHandler<ListHandlerByStateQuery, IEnumerable<HandlerInformation>>
    {
        public IEnumerable<HandlerInformation> Handle(ListHandlerByStateQuery request)
        {
            return HandlerInfos.Where(x => x.State.Equals(request.State, StringComparison.OrdinalIgnoreCase));
        }
    }

}
