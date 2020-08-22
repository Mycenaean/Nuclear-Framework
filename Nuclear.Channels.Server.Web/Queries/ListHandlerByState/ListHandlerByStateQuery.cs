// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Queries.ListHandlerByState
{
    public class ListHandlerByStateQuery
    {
        public string State { get; }

        public ListHandlerByStateQuery(string state)
        {
            State = state;
        }
    }

}
