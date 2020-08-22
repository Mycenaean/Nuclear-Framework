// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.AspNetCoreExtensions
{
    public interface IChannelEndpointProvider
    {
        string GetEndpointUrl(string baseUrl, string methodName);
    }
}
