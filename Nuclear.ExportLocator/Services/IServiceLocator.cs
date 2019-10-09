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
