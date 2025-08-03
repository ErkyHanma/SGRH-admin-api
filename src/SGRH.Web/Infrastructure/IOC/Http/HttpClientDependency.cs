using SGRH.Web.Infrastructure.Http;

namespace SGRH.Web.Infrastructure.IOC.Http
{
    public static class HttpClientDependency
    {
        public static IServiceCollection AddHttpClientDependency(this IServiceCollection services)
        {
            services.AddHttpClient<IHttpClientService, HttpClientService>();
            return services;
        }
    }
}
