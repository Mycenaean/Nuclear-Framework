using System;

namespace Nuclear.Channels.Server.Web.System
{
    public class ReleaseSettings : ISystemSettings
    {
        public string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
    }
}
