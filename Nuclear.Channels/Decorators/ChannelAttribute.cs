using System;

namespace Nuclear.Channels.Decorators
{
    /// <summary>
    /// Attribute that will register specified class as an Channel
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
        /// CTOR
        /// </summary>
        public ChannelAttribute()
        {

        }

        /// <summary>
        /// CTOR with wanted name for the base route
        /// </summary>
        /// <param name="Name">Name of the base route</param>
        public ChannelAttribute(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Channel Description
        /// </summary>
        /// <param name="Description">Description string</param>
        public ChannelAttribute(object Description)
        {
            this.Description = Description;
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
        }

    }
}