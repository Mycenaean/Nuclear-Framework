// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nuclear.Channels.InvokerServices.Contracts
{
    /// <summary>
    /// ChannelMethodDescriptor Contract
    /// </summary>
    public interface IChannelMethodDescriptor
    {
        /// <summary>
        /// Get Input parameters of targeted method
        /// </summary>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <returns>Dictionary with the name and type of the input parameters</returns>
        ChannelMethodInfo GetMethodDescription(MethodInfo method);
    }
}
