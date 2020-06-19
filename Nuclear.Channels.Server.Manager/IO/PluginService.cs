using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Nuclear.Channels.Server.Manager.IO
{
    [Export(typeof(IPluginService), ExportLifetime.Singleton)]
    public class PluginService : IPluginService
    {
        private const string _fileName = "Plugins\\plugins.xml";
        private const string _xmlServer = "server";

        public void Init()
        {
            XmlDocument pluginsXML = new XmlDocument();

            pluginsXML.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName));
            if (!pluginsXML.HasChildNodes)
                throw new PluginsNotFoundException();

            XmlNodeList plugins = pluginsXML.GetElementsByTagName(_xmlServer)[0]?
                                                                .ChildNodes[0]?
                                                                .ChildNodes;
            foreach (XmlNode pluginNode in plugins)
            {
                AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(pluginNode
                                                                                .Attributes["name"]?
                                                                                .Value?
                                                                                .ToString()));
            }
        }
    }
}