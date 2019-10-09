using Nuclear.ExportLocator.Services;

namespace Nuclear.Data.Services
{
    public abstract class ServiceBase
    {
        protected readonly IServiceLocator _services;

        protected ServiceBase()
        {
            _services = ServiceLocator.GetInstance;
        }
    }
}
