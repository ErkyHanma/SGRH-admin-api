using Microsoft.Extensions.DependencyInjection;
using SGRH.Web.Infrastructure.Endpoints.Rate;
using SGRH.Web.Repositories;
using SGRH.Web.Repositories.Interfaces.Hotel;

namespace SGRH.Web.Infrastructure.IOC.Http
{
    public static class RateHttpDependency
    {
        public static IServiceCollection AddRateHttpDependency(this IServiceCollection services)
        {
            services.AddTransient<IRateEndpoints, RateEndpoints>();
            services.AddScoped<IRateApiRepository, RateApiRepository>();
            return services;
        }
    }
}
