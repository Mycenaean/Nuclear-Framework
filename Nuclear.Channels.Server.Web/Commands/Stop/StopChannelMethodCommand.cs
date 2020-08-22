// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Commands.Stop
{
    public class StopChannelMethodCommand
    {
        public string HandlerId { get; set; }

        public StopChannelMethodCommand(string handlerId)
        {
            HandlerId = handlerId;
        }
    }
}
