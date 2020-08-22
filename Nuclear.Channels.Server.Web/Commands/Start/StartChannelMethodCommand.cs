// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Commands.Start
{
    public class StartChannelMethodCommand
    {
        public string HandlerId { get; }

        public StartChannelMethodCommand(string handlerId)
        {
            HandlerId = handlerId;
        }
    }
}
