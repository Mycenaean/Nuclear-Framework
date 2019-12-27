// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Exceptions
{
    /// <summary>
    /// Exception thrown when parameters dont match
    /// </summary>
    public class ChannelMethodParameterException : Exception
    {
        public ChannelMethodParameterException() { }
        public ChannelMethodParameterException(string message) : base(message) { }
        public ChannelMethodParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
