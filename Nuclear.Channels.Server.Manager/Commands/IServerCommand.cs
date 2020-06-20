// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public interface IServerCommand
    {
        void Execute();
    }
}
