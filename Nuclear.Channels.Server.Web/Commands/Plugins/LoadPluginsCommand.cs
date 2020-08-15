using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Web.Commands.Plugins
{
    public class LoadPluginsCommand
    {
        public string ServerPluginsPath { get; }

        public LoadPluginsCommand(string serverPluginsPath)
        {
            ServerPluginsPath = serverPluginsPath;
        }
    }
}
