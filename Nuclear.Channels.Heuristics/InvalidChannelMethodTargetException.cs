// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    /// <summary>
    /// Exception thrown when EnableCacheAttribute is applied to method returning void
    /// </summary>
    public class InvalidChannelMethodTargetException : Exception
    {
        public InvalidChannelMethodTargetException() { }
        public InvalidChannelMethodTargetException(string message) : base(message) { }
    }
}
