// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web
{
    public interface IChannelWebServer
    {
        IChannelServer Server { get; set; }
        IChannelServer ServerCopy { get; }
        void Start();
    }
}