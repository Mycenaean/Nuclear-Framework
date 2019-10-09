using Nuclear.ExportLocator.Enumerations;
using System;

namespace Nuclear.ExportLocator.Decorators
{
    /// <summary>
    /// Attribute that will register service in IServiceLocator
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportAttribute : Attribute
    {
        /// <summary>
        /// Service type
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// Service Lifetime
        /// </summary>
        public ExportLifetime Lifetime { get; set; }

        /// <summary>
        /// Registering Service with Transient Lifetime
        /// </summary>
        /// <param name="service">Type of Service</param>
        public ExportAttribute(Type service)
        {
            ServiceType = service;
        }

        /// <summary>
        /// Registering Service with specified Lifetime
        /// </summary>
        /// <param name="service">Type of Service</param>
        /// <param name="Lifetime">Lifetime of the Service</param>
        public ExportAttribute(Type service, ExportLifetime Lifetime)
        {
            ServiceType = service;
            this.Lifetime = Lifetime;
        }
    }
}
