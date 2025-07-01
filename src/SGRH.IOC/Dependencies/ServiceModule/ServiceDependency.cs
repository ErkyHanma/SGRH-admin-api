using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Common.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Application.Interfaces.Services.Service_Module;
using SGRH.Application.Services.ServiceModule;
using SGRH.Persistence.Repositories.Service_Module;

namespace SGRH.IOC.Dependencies.ServiceModule
{
    public static class ServiceDependency
    {
        public static void AddServiceDependency(this IServiceCollection service)
        {
            service.AddScoped<IServiceRepository, ServiceRepository>();
            service.AddTransient<IServiceService, ServiceService>();
            service.AddScoped<IServiceMapper, ServiceMapper>();
        }
    }
}
