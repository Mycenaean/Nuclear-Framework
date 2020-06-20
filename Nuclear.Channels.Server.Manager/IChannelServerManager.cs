// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Manager.Commands;
using Nuclear.Channels.Server.Manager.Console;
using System.Threading.Tasks;

namespace Nuclear.Channels.Server.Manager
{
    public interface IChannelServerManager
    {
        void Start();        
    }

}