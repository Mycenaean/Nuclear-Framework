// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Linq;

namespace Nuclear.Channels.Server.Web.AspNetCoreExtensions
{
    public class RemoteChannelEndpoints
    {
        public string PluginsChannel => "http://localhost:4200/channels/PluginsChannel/";
        public string ServerChannel => "http://localhost:4200/channels/ServerChannel/";
    }
}
