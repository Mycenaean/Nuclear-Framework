using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Web.Decorators
{
    /// <summary>
    /// Handler that will protect Channel from being Server Managed
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ProtectedHandlerAttribute : Attribute
    {
    }
}
