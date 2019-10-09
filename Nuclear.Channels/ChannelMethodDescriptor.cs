using Nuclear.Channels.Interfaces;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nuclear.Channels
{
    /// <summary>
    /// Service for ChannelMethod informations
    /// </summary>
    [Export(typeof(IChannelMethodDescriptor), Lifetime = ExportLifetime.Transient)]
    public class ChannelMethodDescriptor : IChannelMethodDescriptor
    {
        /// <summary>
        /// Get Input parameters of targeted method
        /// </summary>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <returns>Dictionary with the name and type of the input parameters</returns>
        public Dictionary<string, Type> GetMethodDescription(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            Dictionary<string, Type> inputParams = new Dictionary<string, Type>();
            foreach (var param in parameters)
            {
                inputParams.Add(param.Name, param.ParameterType);
            }

            return inputParams;
        }

    }
}
