// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Queries.HandlerHistory
{
    public class HandlerHistoryQuery
    {
        public string HandlerId { get; set; }

        public HandlerHistoryQuery(string handlerId)
        {
            HandlerId = handlerId;
        }
    }

}
