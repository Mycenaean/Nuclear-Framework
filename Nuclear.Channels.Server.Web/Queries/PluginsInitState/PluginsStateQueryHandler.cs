// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.Common;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.Channels.Server.Web.Abstractions;
using System.Linq;

namespace Nuclear.Channels.Server.Web.Queries.PluginsInitState
{
    [Export(typeof(IEventHandler<PluginsInitStateQuery, PluginsInitState>), ExportLifetime.Scoped)]
    public class PluginsStateQueryHandler : IEventHandler<PluginsInitStateQuery, PluginsInitState>
    {
        public PluginsInitState Handle(PluginsInitStateQuery request)
        {
            var globalExceptionHandler = ServiceFactory.GetExportedService<IGlobalExceptionHandler>();
            var exceptionInfo = globalExceptionHandler.Exceptions.FirstOrDefault(x => x.InvokationMethod == request.MethodName);

            if (exceptionInfo == null)
                return new PluginsInitState(true, null);
            else
                return new PluginsInitState(false, exceptionInfo.Message);
        }
    }

}
