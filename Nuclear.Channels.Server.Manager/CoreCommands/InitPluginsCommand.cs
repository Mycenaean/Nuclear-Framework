// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Manager.Commands;
using Nuclear.Channels.Server.Manager.Exceptions;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Nuclear.Channels.Server.Manager.CoreCommands
{
    public class InitPluginsCommand : IServerCommand
    {
        private string _settingsFileDirectory;
        private string _pluginsDirectory;
        private const string _fileName = "plugins.xml";
        private const string _xmlServer = "server";

        public InitPluginsCommand()
        {
#if DEBUG
            _settingsFileDirectory = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.Parent.Parent.Parent).FullName;
            _pluginsDirectory = Path.Combine(_settingsFileDirectory, "Plugins");
#endif
            _settingsFileDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _pluginsDirectory = Path.Combine(_settingsFileDirectory, "Plugins");
        }

        public void Execute()
        {
            XmlDocument pluginsXML = new XmlDocument();

            pluginsXML.Load(Path.Combine(_settingsFileDirectory, _fileName));
            if (!pluginsXML.HasChildNodes)
                throw new PluginsNotFoundException();

            XmlNodeList plugins = pluginsXML.GetElementsByTagName(_xmlServer)[0]?
                                                                            .ChildNodes[0]?
                                                                            .ChildNodes;

            foreach (XmlNode pluginNode in plugins)
            {
                string pluginFilePath = Path.Combine(_pluginsDirectory, pluginNode.Attributes["name"]?
                                                                                            .Value?
                                                                                            .ToString());

                AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(pluginFilePath));
            }
        }
    }
}
