using SGRH.Application.Common.Logging;
using SGRH.Web.Infrastructure.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SGRH.Web.Repositories.Base
{
    public abstract class BaseApiRepository<TRepository> 
    {
        protected readonly IHttpClientService _httpClientService;
        protected readonly IAppLogger<TRepository> _appLogger;

        protected BaseApiRepository(IHttpClientService httpClientService, IAppLogger<TRepository> appLogger)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
            _appLogger = appLogger ?? throw new ArgumentNullException(nameof(appLogger));
            
        }

        protected async Task<TResponse> ExecuteApiCall<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            Func<string, TRequest, Task<TResponse>> apiCall) // <- Insertar una funcion como parametro, devuelve tipo esperado
            where TResponse : class, new()
        {
            if (request == null)
            {
                _appLogger.ErrorNoEx("Null request provided for {Endpoint}", endpoint);
                return new TResponse();
            }

            try
            {
                var response = await apiCall(endpoint, request);

                if (response == null)
                {
                    _appLogger.ErrorNoEx("Null response received from {Endpoint}", endpoint);
                    return new TResponse();
                }

                LogApiResult(endpoint, response);
                return response;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorEx(ex, "Exception during API call to {Endpoint}", endpoint);
                return new TResponse();
            }
        }

        protected async Task<TResponse> GetAsync<TResponse>(string endpoint) where TResponse : class, new() 
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                _appLogger.ErrorNoEx("Endpoint cannot be null or empty");
                return new TResponse();
            }

            try
            {
                var response = await _httpClientService.GetAsync<TResponse>(endpoint);

                if (response == null)
                {
                    _appLogger.ErrorNoEx("Failed to retrieve list from {Endpoint}: Response was null", endpoint);
                    return new TResponse();
                }

                LogApiResult(endpoint, response);
                return response;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorEx(ex, "Exception during GET all from {Endpoint}", endpoint);
                return new TResponse();
            }
        }
        protected bool IsValidId(int id, string context = "")
        {
            if (id <= 0)
            {
                _appLogger.ErrorNoEx("Invalid ID provided: {Id} in context: {Context}", id, context);
                return false;
            }
            return true;
        }

        private void LogApiResult<TResponse>(string endpoint, TResponse response)
        {
            var responseType = response.GetType();
            var isSuccessProperty = responseType.GetProperty("isSuccess");
            var isSuccessValue = isSuccessProperty?.GetValue(response);

            bool isSuccess = isSuccessValue?.Equals(true) == true;

            if (!isSuccess)
            {
                var messageProperty = responseType.GetProperty("message");
                var errorMessage = messageProperty?.GetValue(response)?.ToString();
                _appLogger.ErrorNoEx("API call failed to {Endpoint}: {message}", endpoint, errorMessage);
            }
            else
            {
                _appLogger.Info("API call succeeded to {Endpoint}", endpoint);
            }

        }
    }
}
