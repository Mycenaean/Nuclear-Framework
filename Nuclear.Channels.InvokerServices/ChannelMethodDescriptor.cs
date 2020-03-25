// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using Nuclear.Channels.InvokerServices.Contracts;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nuclear.Channels.InvokerServices
{
    /// <summary>
    /// Service for ChannelMethod informations
    /// </summary>
    [Export(typeof(IChannelMethodDescriptor), Lifetime = ExportLifetime.Transient)]
    public class ChannelMethodDescriptor : IChannelMethodDescriptor
    {
        public ChannelMethodInfo GetMethodDescription(MethodInfo method)
        {
            ChannelMethodInfo channelMethodInfo = new ChannelMethodInfo();
            ParameterInfo[] parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                channelMethodInfo.AddParameter(param.Name, param.ParameterType);
            }

            return channelMethodInfo;
        }

    }
}
