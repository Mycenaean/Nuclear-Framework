// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channel.Generators.Exceptions
{
    public class ImportFailedException : Exception
    {
        public ImportFailedException() { }
        public ImportFailedException(string message) : base(message) { }
        public ImportFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
