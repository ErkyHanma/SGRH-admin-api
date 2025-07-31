using SGRH.IOC;

namespace SGRH.Web.Infrastructure.Http
{
    public interface IHttpClientService
    {
        Task<TResponse> GetAsync<TResponse>(string endpoint) where TResponse : class;

        Task<TResponse> PostAsync<TResponse, TRequest>(string endpoint, TRequest request) where TResponse : class;

        Task<TResponse> PutAsync<TResponse, TRequest>(string endpoint, TRequest request) where TResponse : class;

    }
}
