using Microsoft.Extensions.DependencyInjection;
using Nuclear.ExportLocator.Services;

namespace Nuclear.ExportLocator.Extensions
{
    public static class ServiceLocatorExtensions
    {
        public static IServiceCollection AddServicesFromServiceLocator(this IServiceCollection services)
        {
            var locator = ServiceLocator.GetInstance;
            var locatorServices = locator.Services;

            using var serviceEnumerator = locatorServices.GetEnumerator();
            services.Add(serviceEnumerator.Current);

            while (serviceEnumerator.MoveNext())
                services.Add(serviceEnumerator.Current);

            return services;
        }
    }
}
