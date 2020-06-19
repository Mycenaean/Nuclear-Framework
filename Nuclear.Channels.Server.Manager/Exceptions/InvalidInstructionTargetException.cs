using System;

namespace Nuclear.Channels.Server.Manager.Exceptions
{
    public class InvalidInstructionTargetException : Exception 
    {
        public InvalidInstructionTargetException() { }
        public InvalidInstructionTargetException(string message) : base(message) { }
    }
}
