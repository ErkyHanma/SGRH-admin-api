using SGRH.Infrastructure.Common.Logging;
using System.Text.Json;

namespace SGRH.Web.Infrastructure.Http
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppLogger<HttpClientService> _logger;
        private readonly JsonSerializerOptions _jsonSerializer;

        public HttpClientService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IAppLogger<HttpClientService> logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("ApiSettings:BaseUrl is not configured");

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseUrl);

            _jsonSerializer = new JsonSerializerOptions();
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint) where TResponse : class
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                return CreateErrorResponse<TResponse>("Endpoint cannot be null or empty");
            
            try
            {
                _logger.Info("Making GET request to: {Endpoint}", endpoint);

                var response = await _httpClient.GetAsync(endpoint);

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<TResponse>(responseString, _jsonSerializer);
                    return result ?? CreateErrorResponse<TResponse>("Failed to deserialize response");
                }
                _logger.ErrorNoEx("GET request failed to {Endpoint} with status code: {StatusCode}", endpoint, response.StatusCode);
                return CreateErrorResponse<TResponse>("Error retrieving data from API");
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception occurred during GET request to: {Endpoint}", endpoint);
                return CreateErrorResponse<TResponse>($"Internal error: {ex.Message}");
            }
        }

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string endpoint, TRequest request) where TResponse : class
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                return CreateErrorResponse<TResponse>("Endpoint cannot be null or empty");

            if (request == null)
                return CreateErrorResponse<TResponse>("Request body cannot be null");

            try
            {
                _logger.Info("Making POST request to: {Endpoint}", endpoint);

                var response = await _httpClient.PostAsJsonAsync(endpoint, request);
                var responseString = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TResponse>(responseString, _jsonSerializer);             
                return result ?? CreateErrorResponse<TResponse>("Failed to deserialize response");
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception occurred during POST request to: {Endpoint}", endpoint);
                return CreateErrorResponse<TResponse>($"Internal error: {ex.Message}");
            }
        }

        public async Task<TResponse> PutAsync<TResponse, TRequest>(string endpoint, TRequest request) where TResponse : class
        {

            if (string.IsNullOrWhiteSpace(endpoint))
                return CreateErrorResponse<TResponse>("Endpoint cannot be null or empty");

            if (request == null)
                return CreateErrorResponse<TResponse>("Request body cannot be null");

            try
            {
                _logger.Info("Making PUT request to: {Endpoint}", endpoint);

                var response = await _httpClient.PutAsJsonAsync(endpoint, request);
                var responseString = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TResponse>(responseString, _jsonSerializer);
                return result ?? CreateErrorResponse<TResponse>("Failed to deserialize response");

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception occurred during PUT request to: {Endpoint}", endpoint);
                return CreateErrorResponse<TResponse>($"Internal error: {ex.Message}");
            }
        }

        private TResponse CreateErrorResponse<TResponse>(string message) where TResponse : class
        {
            var response = Activator.CreateInstance<TResponse>();
            var isSuccessProperty = typeof(TResponse).GetProperty("isSuccess");
            var messageProperty = typeof(TResponse).GetProperty("message");

            isSuccessProperty?.SetValue(response, false);
            messageProperty?.SetValue(response, message);

            return response;
        }
    }
}
