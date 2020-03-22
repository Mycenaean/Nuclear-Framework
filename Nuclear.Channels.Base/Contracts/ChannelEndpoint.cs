// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Base.Contracts
{
    /// <summary>
    /// Http Endpoint to be initialized
    /// </summary>
    public class ChannelEndpoint : IChannelEndpoint
    {
        public string URL { get; set; }
        public string Name { get; set; }

    }
}
