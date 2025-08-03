using Microsoft.Extensions.DependencyInjection;
using SGRH.Web.Infrastructure.Endpoints.Room;
using SGRH.Web.Repositories;
using SGRH.Web.Repositories.Interfaces.Hotel;

namespace SGRH.Web.Infrastructure.IOC.Http
{
    public static class RoomHttpDependency
    {
        public static IServiceCollection AddRoomHttpDependency(this IServiceCollection services)
        {
            services.AddTransient<IRoomEndpoints, RoomEndpoints>();
            services.AddScoped<IRoomApiRepository, RoomApiRepository>();
            return services;
        }
    }
}
