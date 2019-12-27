// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Messaging.Services.ChannelMessage;

namespace Nuclear.Channels.Messaging
{
    /// <summary>
    /// Base return type of HttpListenerResponse
    /// </summary>
    public class ChannelMessage : IChannelMessage
    {
        public bool Success { get; set; }
        public object Output { get; set; }
        public string Message { get; set; }

    }
}
