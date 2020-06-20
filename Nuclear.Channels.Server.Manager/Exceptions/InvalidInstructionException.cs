// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager.Exceptions
{
    public class InvalidInstructionException : Exception
    {
        public InvalidInstructionException() { }
        public InvalidInstructionException(string message) : base(message) { }
    }
}
