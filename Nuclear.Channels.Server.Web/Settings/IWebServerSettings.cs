using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Web.Settings
{
    public interface IWebServerSettings
    {
        IEnumerable<UserSettings> Users { get; }
        IEnumerable<ChannelSettings> Channels { get; }
    }
}
