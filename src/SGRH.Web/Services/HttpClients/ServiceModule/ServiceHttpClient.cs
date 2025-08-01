using SGRH.Application.Common.Logging;
using SGRH.Web.Interfaces.HttpClients.ServiceModule;
using SGRH.Web.Models;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;
using System.Text.Json;

namespace SGRH.Web.Services.HttpClients.ServiceModule
{
    public class ServiceHttpClient : IServiceHttpClient
    {

        private readonly HttpClient _client;
        private readonly IAppLogger<ServiceHttpClient> _logger;



        public ServiceHttpClient(HttpClient client, IAppLogger<ServiceHttpClient> logger)
        {
            _client = client;
            _logger = logger;

        }

        public async Task<GetAllServicesResponse> GetAllServicesAsync()
        {

            try
            {
                var response = await _client.GetAsync("Service");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<GetAllServicesResponse>(responseString);

                }
                else
                {
                    return new GetAllServicesResponse { isSuccess = false, message = "Error retrieving services." };
                }
            }

            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while retrieving services.");
                return new GetAllServicesResponse { isSuccess = false, message = "Error retrieving services." };
            }

        }
        public async Task<BaseResponse<TModel>> GetServiceByIdAsync<TModel>(int id)
        {
            try
            {
                var response = await _client.GetAsync($"Service/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<BaseResponse<TModel>>(responseString);
                }
                else
                {
                    return new BaseResponse<TModel> { isSuccess = false, message = "Error retrieving services." };
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while retrieving service.");
                return new BaseResponse<TModel> { isSuccess = false, message = "Error retrieving service." };
            }
        }
        public async Task<CreateServiceResponse> CreateServiceAsync(CreateServiceModel createServiceModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync("Service/CreateService", createServiceModel);
                var responseContent = await response.Content.ReadAsStringAsync();
                var createServiceResponse = JsonSerializer.Deserialize<CreateServiceResponse>(responseContent);

                if (response.IsSuccessStatusCode && createServiceResponse is not null)
                {
                    return createServiceResponse;
                }
                else
                {
                    return new CreateServiceResponse { isSuccess = false, message = createServiceResponse?.message ?? "Error creating services." };
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while creating services.");
                return new CreateServiceResponse { isSuccess = false, message = "Error creating services." };
            }

        }
        public async Task<EditServiceResponse> EditServiceAsync(ServiceModel serviceModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync($"Service/UpdateService", serviceModel);
                var responseContent = await response.Content.ReadAsStringAsync();
                var editServiceResponse = JsonSerializer.Deserialize<EditServiceResponse>(responseContent);

                if (response.IsSuccessStatusCode && editServiceResponse is not null)
                {
                    return editServiceResponse;
                }
                else
                {
                    return new EditServiceResponse { isSuccess = false, message = editServiceResponse?.message ?? "Error modifying service." };
                }

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred modifying service");
                return new EditServiceResponse
                {
                    isSuccess = false,
                    message = $"Error while creating service. {ex.Message}"
                };
            }
        }
        public async Task<DeleteServiceResponse> DeleteServiceAsync(DeleteServiceModel deleteServiceModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync($"Service/DisableService", deleteServiceModel);
                var responseContent = await response.Content.ReadAsStringAsync();
                var deleteServiceResponse = JsonSerializer.Deserialize<DeleteServiceResponse>(responseContent);

                if (response.IsSuccessStatusCode && deleteServiceResponse is not null)
                {
                    return deleteServiceResponse;
                }
                else
                {
                    return new DeleteServiceResponse { isSuccess = false, message = deleteServiceResponse?.message ?? "Error while deleting service." };
                }


            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred deleting service");
                return new DeleteServiceResponse { isSuccess = false, message = "Error while deleting service" };
            }
        }


    }
}


