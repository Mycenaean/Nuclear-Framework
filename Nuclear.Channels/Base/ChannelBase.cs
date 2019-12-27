// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Base
{
    /// <summary>
    /// Base abstract WebChannel Helper class
    /// </summary>
    public class ChannelBase : IChannel
    {
        /// <summary>
        /// ServiceLocator
        /// </summary>
        public IServiceLocator Services => ServiceLocatorBuilder.CreateServiceLocator();
        
    }
}
