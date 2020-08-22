// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Commands.Restart
{
    public class RestartChannelMethodCommand
    {
        public string HandlerId { get; }

        public RestartChannelMethodCommand(string handlerId)
        {
            HandlerId = handlerId;
        }
    }
}
