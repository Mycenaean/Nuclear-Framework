// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Collections.Generic;
using Nuclear.ExportLocator.Services;

namespace Nuclear.ExportLocator
{
    /// <summary>
    /// ServiceLocator builder
    /// </summary>
    public static class ServiceLocatorBuilder
    {
        /// <summary>
        /// Method that will create IServiceLocator instance
        /// </summary>
        public static IServiceLocator CreateServiceLocator()
        {
            return ServiceLocator.GetInstance;
        }

        /// <summary>
        /// Method that will return IServiceLocator instance
        /// </summary>
        /// <remarks>
        /// Use this if you know that this is the first ServiceLocator creation instance because it will
        /// speed up exports lookup time
        /// </remarks>
        /// <param name="assemblies">Assemblies containing exported services</param>
        public static IServiceLocator CreateServiceLocator(List<string> assemblies)
        {
            return new ServiceLocator(assemblies);
        }
    }
}
