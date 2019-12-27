// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Decorators
{
    /// <summary>
    /// Attribute that will register specified class as an Channel, 
    /// If Name property is not set , targeted class name will be used as a base route
    /// </summary>    
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ChannelAttribute : Attribute
    {
        /// <summary>
        /// Name that will be used as a base route endpoint if specified
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description provided for documentation
        /// </summary>
        public object Description { get; set; }

        /// <summary>
        /// Enable Sessions 
        /// </summary>
        /// <remarks>Default is false</remarks>
        public bool EnableSessions { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public ChannelAttribute()
        {
            EnableSessions = false;
        }

        /// <summary>
        /// CTOR with wanted name for the base route
        /// </summary>
        /// <param name="Name">Name of the base route</param>
        public ChannelAttribute(string Name)
        {
            this.Name = Name;
            EnableSessions = false;
        }

        /// <summary>
        /// Channel Description
        /// </summary>
        /// <param name="Description">Description string</param>
        public ChannelAttribute(object Description)
        {
            this.Description = Description;
            EnableSessions = false;
        }


        /// <summary>
        /// Channel Sessions
        /// </summary>
        /// <param name="EnableSessions">Enable session storing</param>
        public ChannelAttribute(bool EnableSessions)
        {
            this.EnableSessions = EnableSessions;
        }

        /// <summary>
        /// Channel Name with Description
        /// </summary>
        /// <param name="Name">Name of the base route</param>
        /// <param name="Description">Description string</param>
        public ChannelAttribute(string Name, object Description)
        {
            this.Name = Name;
            this.Description = Description;
            EnableSessions = false;
        }

        /// <summary>
        /// Channel Name with Sessions
        /// </summary>
        /// <param name="Name">Name of the base route</param>
        /// <param name="EnableSessions">Enable session storing</param>
        public ChannelAttribute(string Name, bool EnableSessions)
        {
            this.Name = Name;
            this.EnableSessions = EnableSessions;
        }

        /// <summary>
        /// Channel Description with Sessions
        /// </summary>
        /// <param name="Description">Description string</param>
        /// <param name="EnableSessions">Enable session storing</param>
        public ChannelAttribute(object Description, bool EnableSessions)
        {
            this.Description = Description;
            this.EnableSessions = EnableSessions;
        }


        /// <summary>
        /// Channel Name with Description and Session storing
        /// </summary>
        /// <param name="Name">Name of the base route</param>
        /// <param name="Description">Description string</param>
        /// <param name="EnableSessions">Enable session storing</param>
        public ChannelAttribute(string Name, object Description, bool EnableSessions)
        {
            this.Name = Name;
            this.Description = Description;
            this.EnableSessions = this.EnableSessions;
        }
    }
}