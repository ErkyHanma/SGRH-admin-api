
using Microsoft.Extensions.DependencyInjection;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Interfaces.HttpClients.ServiceModule;
using SGRH.Web.Services.HttpClients.ReservationModule;
using SGRH.Web.Services.HttpClients.ServiceModule;





namespace SGRH.Presentation.IOC.HttpClient

{
    public static class HttpClientDependencies
    {
        public static void AddHttpClientDependencies(this IServiceCollection service)
        {
            service.AddTransient<IServiceHttpClient, ServiceHttpClient>();
            service.AddTransient<IReservationHttpClient, ReservationHttpClient>();
            service.AddTransient<IReservationServiceHttpClient, ReservationServiceHttpClient>();

        }

    }
}
