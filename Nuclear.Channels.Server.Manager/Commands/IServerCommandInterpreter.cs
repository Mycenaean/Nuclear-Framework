﻿// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Manager.Commands
{
    public interface IServerCommandInterpreter
    {
        ServerCommandType InterpretType(string type);
        ServerCommandTarget InterpretTarget(string handlerId);
    }
}
