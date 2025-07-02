using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Application.Services.ReservationModule;
using SGRH.Persistence.Repositories.ReservationModule;

namespace SGRH.IOC.Dependencies.ReservationModule
{
    public static class ReservationDependency
    {
        public static void AddReservationDependency(this IServiceCollection service)
        {
            service.AddScoped<IReservationRepository, ReservationRepository>();
            service.AddTransient<IReservationService, ReservationService>();
        }
    }
}
