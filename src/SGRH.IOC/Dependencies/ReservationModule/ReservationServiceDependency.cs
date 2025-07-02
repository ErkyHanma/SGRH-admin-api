using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Application.Services.ReservationModule;
using SGRH.Persistence.Repositories.ReservationModule;

namespace SGRH.IOC.Dependencies.ReservationModule
{
    public static class ReservationServiceDependency
    {
        public static void AddReservationServiceDependency(this IServiceCollection service)
        {
            service.AddScoped<IReservationServiceRepository, ReservationServiceRepository>();
            service.AddTransient<IReservationServiceService, ReservationServiceService>();
        }
    }
}
