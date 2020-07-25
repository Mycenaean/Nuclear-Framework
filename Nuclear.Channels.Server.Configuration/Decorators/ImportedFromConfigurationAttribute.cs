using System;

namespace Nuclear.Channels.Server.Configuration.Decorators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class ImportedFromConfigurationAttribute : Attribute
    {
        
    }
}