// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.ExportLocator.Services
{
    /// <summary>
    /// Service Locator Container
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Get the service from IServiceLocator
        /// </summary>
        /// <typeparam name="T">Requested Interface</typeparam>
        /// <returns>Specified service</returns>
        T Get<T>();
    }
}
