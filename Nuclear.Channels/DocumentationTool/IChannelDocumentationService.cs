using System;
using System.Collections.Generic;

namespace Nuclear.Channels.DocumentationTool
{
    /// <summary>
    /// Service that autogenerates documentation for all created channels
    /// </summary>
    public interface IChannelDocumentationService
    {
        List<ChannelDocument> GetDocumentation(AppDomain domain);
    }
}
