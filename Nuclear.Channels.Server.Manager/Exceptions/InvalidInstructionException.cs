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
