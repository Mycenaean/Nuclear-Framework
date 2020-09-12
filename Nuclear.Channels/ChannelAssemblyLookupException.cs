// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels
{
    /// <summary>
    /// Exception thrown when Assemblies could not be loaded from lookup assemblies
    /// </summary>
    public class ChannelAssemblyLookupException : Exception
    {
        public ChannelAssemblyLookupException(string message) : base(message) { }
    }
}
