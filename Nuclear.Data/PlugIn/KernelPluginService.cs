using Nuclear.Data.PlugIn.Contracts;
using Nuclear.ExportLocator.Decorators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Nuclear.Data.PlugIn
{
    /// <summary>
    /// Service that will get all plugin channels and load assemblies to the host
    /// </summary>

    [Export(typeof(IKernelPluginService))]
    public class KernelPluginService : IKernelPluginService
    {
        /// <summary>
        /// Requires testing , probably not needed anymore
        /// </summary>
        /// <param name="domain">Application domain</param>
        /// <returns>List of Channels</returns>
        [Obsolete("Dont use this method since it is not required. Use use LoadPlugIns")]
        public List<Type> GetPlugins(AppDomain domain)
        {
            string RootPath = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName + "\\PlugIns";

            List<Type> Channels = new List<Type>();
            foreach (var file in Directory.GetFiles(RootPath, "*.dll"))
            {
                var asm = Assembly.LoadFrom(file);
                domain.Load(asm.FullName);
                foreach (var type in asm.GetTypes())
                {
                    var pluginDll = type.GetInterface("IKernelPlugIn");
                    if (pluginDll == null)
                        continue;
                    IKernelPlugIn plugin = Activator.CreateInstance(type) as IKernelPlugIn;
                    Channels.AddRange(plugin.LoadChannels());
                }
            }

            return Channels;
        }

        /// <summary>
        /// Loads assemblies from the PlugIns folder
        /// </summary>
        /// <param name="domain">Application domain</param>
        public void LoadPlugIns(AppDomain domain)
        {
            string RootPath = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName + "\\PlugIns";

            List<Type> Channels = new List<Type>();
            foreach (var file in Directory.GetFiles(RootPath, "*.dll"))
            {
                var asm = Assembly.LoadFrom(file);
                domain.Load(asm.FullName);
            }
        }
    }
}