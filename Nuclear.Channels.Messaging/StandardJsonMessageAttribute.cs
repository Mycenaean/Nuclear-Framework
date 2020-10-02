using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Messaging
{
    /// <summary>
    /// Decorated Channel Method or response object will not be serialized into ChannelMessage
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class StandardJsonMessageAttribute : Attribute
    {
    }
}
