using Nuclear.ExportLocator.Enumerations;
using System;

namespace Nuclear.ExportLocator.Global
{
    /// <summary>
    /// Exported Service Description
    /// </summary>
    internal class ExportInformation
    {
        /// <summary>
        /// Decorated Class
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// Service Interface
        /// </summary>
        public Type Implementation { get; set; }

        /// <summary>
        /// Service Lifetime
        /// </summary>
        public ExportLifetime ExportLifetime { get; set; }
    }
}
