﻿// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Server.Manager.Exceptions
{
    public class InvalidInstructionTargetException : Exception 
    {
        public InvalidInstructionTargetException() { }
        public InvalidInstructionTargetException(string message) : base(message) { }
    }
}
