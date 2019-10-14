using Nuclear.Channels.Contracts;
using Nuclear.Channels.Decorators;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Nuclear.Channels
{
    /// <summary>`
    /// Service that contains all Channels
    /// </summary>
    [Export(typeof(IChannelLocator), ExportLifetime.Transient)]
    internal class ChannelLocator : IChannelLocator
    {
        private List<Type> Channels = new List<Type>();

        /// <summary>
        /// Method that get all Channels
        /// </summary>
        /// <param name="domain">Domain with all assemblies</param>
        /// <returns>List of classes that are decorated with ChannelAttribute</returns>
        public List<Type> RegisteredChannels(AppDomain domain)
        {
            Debug.Assert(domain != null);

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {
                foreach (var type in asm.GetTypes())
                {
                    Attribute channel = type.GetCustomAttribute(typeof(ChannelAttribute));
                    if (channel != null)
                        Channels.Add(type);
                }
            }
            return Channels;
        }
    }
}
