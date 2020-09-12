// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace Nuclear.Channels.Base.Contracts
{
    /// <summary>
    /// Service that contains all Channels
    /// </summary>
    public interface IChannelLocator
    {

        /// <summary>
        /// Method that get all Channels
        /// </summary>
        /// <param name="domain">Domain with all assemblies</param>
        /// <returns>List of classes that are decorated with ChannelAttribute</returns>
        List<Type> RegisteredChannels(AppDomain domain);

        /// <summary>
        /// Method that get all Channels
        /// </summary>
        /// <param name="lookupAssemblies">List of lookup assemblies</param>
        /// <returns>List of classes that are decorated with ChannelAttribute</returns>
        List<Type> RegisteredChannels(List<string> lookupAssemblies);
    }
}
