// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels
{
    public interface IServerManaged
    {
        /// <summary>
        /// Method indicating is IChannelServer instance managed by the ChannelServerManager
        /// </summary>
        void IsServerManaged(bool managed = false);
    }
}
