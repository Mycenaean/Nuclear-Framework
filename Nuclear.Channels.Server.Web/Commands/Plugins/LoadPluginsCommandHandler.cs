using Nuclear.Channels.Server.Web.Common;
using Nuclear.Channels.Server.Web.Abstractions;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Nuclear.Channels.Server.Web.Commands.Plugins
{
    [Export(typeof(IEventHandler<LoadPluginsCommand>), ExportLifetime.Scoped)]
    public class LoadPluginsCommandHandler : EventHandlerBase, IEventHandler<LoadPluginsCommand>
    {
        private readonly IGlobalExceptionHandler _globalExceptionHandler;

        private const string _pluginsDir = "Plugins";
        private const string _invokationMethod = "InitPlugins";
        public bool ServerPluginsInitialized { get; private set; }

        public LoadPluginsCommandHandler()
        {
            _globalExceptionHandler = ServiceFactory.GetExportedService<IGlobalExceptionHandler>();
        }

        public void Handle(LoadPluginsCommand request)
        {

            if (!Directory.Exists(request.ServerPluginsPath))
            {
                var exception =  new PluginsDirectoryException("Plugins directory not found");
                _globalExceptionHandler.AddExceptionInformation(exception,_invokationMethod);
                return;
            }

            var executingAssemblyPath = Assembly.GetExecutingAssembly().FullName;
            var pluginsPath = Path.Combine(executingAssemblyPath, _pluginsDir);

            var serverPluginsDirectory = new DirectoryInfo(request.ServerPluginsPath);

            if(serverPluginsDirectory == null)
            {
                var exception = new PluginsDirectoryException("Plugins directory not found");
                _globalExceptionHandler.AddExceptionInformation(exception, _invokationMethod);
                return;
            }

            var pluginsDirectory = new DirectoryInfo(pluginsPath);

            if (!pluginsDirectory.Exists) Directory.CreateDirectory(pluginsPath);


            var plugins = serverPluginsDirectory.GetFiles();
            try
            {
                foreach (var plugin in plugins)
                {
                    var copyPluginPath = Path.Combine(pluginsPath, plugin.Name);
                    File.Copy(plugin.FullName, copyPluginPath);

                    AppDomain.CurrentDomain.Load(copyPluginPath);
                }

                //Dispose all ChannelMethods
                RawHandlers.ForEach(handler => handler.Dispose());

                //Get WebServer instance
                var webServer = ServiceFactory.GetExportedService<IChannelWebServer>();

                //Since user is defining IChannelServer options its important that they stay the same so we need to override
                //Server instace with the Copy of it which will be created by WebServer itself on first initialization
                webServer.Server = null;
                webServer.Server = webServer.ServerCopy;
                webServer.Start();

                var failedBefore = _globalExceptionHandler.Exceptions
                    .FirstOrDefault(x => x.InvokationMethod == _invokationMethod) != null;
                
                if (failedBefore)
                    _globalExceptionHandler.RemoveExceptionInformation(_invokationMethod);

            }
            catch(Exception ex)
            {
                _globalExceptionHandler.AddExceptionInformation(ex, _invokationMethod);
            }
            
        }
    }
}
