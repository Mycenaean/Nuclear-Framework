// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Authentication
{
    public class ChannelCredentialsException : Exception
    {
        public ChannelCredentialsException() { }
        public ChannelCredentialsException(string message) : base(message) { }
    }
}
