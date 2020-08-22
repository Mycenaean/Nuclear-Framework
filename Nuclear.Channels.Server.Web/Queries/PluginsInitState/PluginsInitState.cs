// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Queries.PluginsInitState
{
    public class PluginsInitState
    {
        public bool Success { get; }
        public string Error { get; }

        public PluginsInitState(bool success,string error)
        {
            Success = success;
            Error = error;
        }
    }

}
