// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Base.Exceptions
{
    /// <summary>
    /// Exception thrown when Content-Type is neither application/json nor application/xml
    /// </summary>
    public class ChannelMethodContentTypeException : Exception
    {
        public ChannelMethodContentTypeException() { }
        public ChannelMethodContentTypeException(string message) : base(message) { }
        public ChannelMethodContentTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
