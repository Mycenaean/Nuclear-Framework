using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Web.Queries.PluginsInitState
{
    public class PluginsInitStateQuery
    {
        public string MethodName { get; }

        public PluginsInitStateQuery()
        {
            MethodName = "InitPlugins";
        }
    }

}
