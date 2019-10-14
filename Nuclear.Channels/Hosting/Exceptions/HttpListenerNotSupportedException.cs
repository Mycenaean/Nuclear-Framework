using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Exceptions
{
    /// <summary>
    /// Exception that is thrown when HttpListener is not supported
    /// </summary>
    public class HttpListenerNotSupportedException : Exception
    {
        public HttpListenerNotSupportedException() { }
        public HttpListenerNotSupportedException(string message) : base(message) { }
        public HttpListenerNotSupportedException(string message, Exception inner) : base(message, inner) { }
    }
}
