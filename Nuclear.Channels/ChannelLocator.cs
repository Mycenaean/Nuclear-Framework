// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Contracts;
using Nuclear.Channels.Decorators;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels
{
    /// <summary>
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
            if (domain == null)
                throw new ArgumentNullException(nameof(AppDomain));

            Assembly[] assemblies = domain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                foreach (Type type in asm.GetTypes())
                {
                    Attribute channel = type.GetCustomAttribute<ChannelAttribute>();
                    if (channel != null)
                        Channels.Add(type);
                }
            }
            return Channels;
        }
    }
}
