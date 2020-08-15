using System;
using System.IO;

namespace Nuclear.Channels.Server.Web.System
{
    public class DebugSettings : ISystemSettings
    {
        public string BaseDirectory => (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName;
    }
}
