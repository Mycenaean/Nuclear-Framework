// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Generators
{
    /// <summary>
    /// Attribute which will import service for decorated interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ImportedServiceAttribute : Attribute
    {
    }
}
