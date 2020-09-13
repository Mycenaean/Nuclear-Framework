// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base.Contracts;
using Nuclear.Channels.Decorators;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels
{
    /// <summary>
    /// Service that contains all Channels
    /// </summary>
    [Export(typeof(IChannelLocator), ExportLifetime.Singleton)]
    internal class ChannelLocator : IChannelLocator
    {
        private List<Type> _channels = new List<Type>();

        /// <summary>
        /// Method that get all Channels
        /// </summary>
        /// <param name="domain">Domain with all assemblies</param>
        /// <returns>List of classes that are decorated with ChannelAttribute</returns>
        public List<Type> RegisteredChannels(AppDomain domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(AppDomain));

            Assembly[] assemblies = domain.GetAssemblies();

            return GetChannels(assemblies);
        }

        /// <summary>
        /// Method that get all Channels
        /// </summary>
        /// <param name="lookupAssemblies">List of lookup assemblies</param>
        /// <returns>List of classes that are decorated with ChannelAttribute</returns>
        public List<Type> RegisteredChannels(List<string> lookupAssemblies)
        {
            if(lookupAssemblies == null || lookupAssemblies.Count == 0)
                throw new ChannelAssemblyLookupException("Empty List<string> lookup assemblies passed");

            Assembly[] lookupAsm = new Assembly[lookupAssemblies.Count];

            for(int i = 0; i < lookupAssemblies.Count; i++)
            {
                lookupAsm[i] = Assembly.Load(lookupAssemblies[i]);
            }

            if (!lookupAsm.Any())
                throw new ChannelAssemblyLookupException("Could not load Assemblies, make sure that your project is referencing them");

            return GetChannels(lookupAsm);
        }
       
        private List<Type> GetChannels(Assembly[] assemblies)
        {
            foreach (Assembly asm in assemblies)
            {
                foreach (Type type in asm.GetTypes())
                {
                    Attribute channel = type.GetCustomAttribute<ChannelAttribute>();
                    if (channel != null)
                        _channels.Add(type);
                }
            }
            return _channels;
        }
    }
}
