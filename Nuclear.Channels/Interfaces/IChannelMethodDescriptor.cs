using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nuclear.Channels.Interfaces
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
        Dictionary<string, Type> GetMethodDescription(MethodInfo method);
    }
}
