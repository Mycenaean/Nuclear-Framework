// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Generators.Exceptions
{
    /// <summary>
    /// Exception thrown when requested service does not contain implementation in IServiceLocator
    /// </summary>
    public class ImportFailedException : Exception
    {
        public ImportFailedException() { }
        public ImportFailedException(string message) : base(message) { }
        public ImportFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
