// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Base.Exceptions
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
