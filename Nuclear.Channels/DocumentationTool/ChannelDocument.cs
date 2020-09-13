// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Collections.Generic;

namespace Nuclear.Channels.DocumentationTool
{
    /// <summary>
    /// Object containing all information about Channel
    /// </summary>
    public class ChannelDocument
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ChannelMethodDocument> AvailableEndpoints { get; set; }

    }
}
