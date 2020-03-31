// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Authentication.Decorators
{
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Property)]
    public sealed class DelegateInfoAttribute : Attribute
    {
        public Type DelegateType { get; set; }

        public DelegateInfoAttribute(Type delegateType)
        {
            DelegateType = delegateType;
        }
    }
}
