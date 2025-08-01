using SGRH.Application.Common.Logging;
using SGRH.Web.Interfaces.HttpClients;
using System.Text.Json;

namespace SGRH.Web.Services.HttpClients
{
    public class BaseHttpClientMethods : IBaseHttpClientMethods
    {
        private readonly HttpClient _client;
        protected readonly IAppLogger<BaseHttpClientMethods> _logger;

        public BaseHttpClientMethods(HttpClient client, IAppLogger<BaseHttpClientMethods> logger)
        {
            _client = client;
            _logger = logger;
        }

        // GET
        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _client.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(content) ?? throw new JsonException("Response deserialization returned null.");

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, $"GET {endpoint} failed.");
                throw;
            }
        }

        // POST
        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(endpoint, data);
                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(content) ?? throw new JsonException("Response deserialization returned null.");
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, $"POST {endpoint} failed");
                throw; // La excepcion se maneja en el servicio
            }
        }
    }
}


