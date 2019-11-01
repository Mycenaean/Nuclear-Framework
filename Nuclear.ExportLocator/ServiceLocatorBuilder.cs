using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.ExportLocator
{
    /// <summary>
    /// ServiceLocator builder
    /// </summary>
    public class ServiceLocatorBuilder
    {
        /// <summary>
        /// Method that will create IServiceLocator instance
        /// </summary>
        /// <returns></returns>
        public static IServiceLocator CreateServiceLocator()
        {
            return ServiceLocator.GetInstance;
        }
    }
}
