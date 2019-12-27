// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Contracts
{
    /// <summary>
    /// Base contract for abstract ChannelBase class
    /// </summary>
    public interface IChannel
    {
        IServiceLocator Services { get; }
    }
}

