using SGRH.Application.Common.Logging;
using SGRH.Web.Interfaces.HttpClients;
using SGRH.Web.Interfaces.HttpClients.ServiceModule;
using SGRH.Web.Models;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;
using SGRH.Web.Models.ServiceModule.Validation;

namespace SGRH.Web.Services.HttpClients.ServiceModule
{
    public class ServiceHttpClient : IServiceHttpClient
    {

        private readonly IBaseHttpClientMethods _httpClient;
        private readonly IAppLogger<ServiceHttpClient> _logger;



        public ServiceHttpClient(IAppLogger<ServiceHttpClient> logger, IBaseHttpClientMethods httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<GetAllServicesResponse> GetAllServicesAsync()
        {

            try
            {
                var response = await _httpClient.GetAsync<GetAllServicesResponse>("Service");

                return response ?? new GetAllServicesResponse
                {
                    isSuccess = false,
                    message = $"Error retieving services."
                };
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

                if (id < 0)
                {
                    return new BaseResponse<TModel>
                    {
                        isSuccess = false,
                        message = "The provided ID is invalid."
                    };
                }


                var response = await _httpClient.GetAsync<BaseResponse<TModel>>($"Service/{id}");

                return response ?? new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = $"Error retieving services."
                };
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
                CreateServiceModelValidator createServiceModelValidation = new();
                var validationResult = createServiceModelValidation.Validate(createServiceModel);


                if (!validationResult.isSuccess)
                {
                    return validationResult;
                }

                var createServiceResponse = await _httpClient.PostAsync<CreateServiceResponse>("Service/CreateService", createServiceModel);

                return createServiceResponse ?? new CreateServiceResponse
                {
                    isSuccess = false,
                    message = $"{createServiceResponse.message ?? "Error creating services."}"
                };
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
                EditServiceModelValidator editServiceModelValidation = new();
                var validationResult = editServiceModelValidation.Validate(serviceModel);


                if (!validationResult.isSuccess)
                {
                    return validationResult;
                }


                var editServiceResponse = await _httpClient.PostAsync<EditServiceResponse>("Service/UpdateService", serviceModel);

                return editServiceResponse ?? new EditServiceResponse
                {
                    isSuccess = false,
                    message = $"{editServiceResponse.message ?? "Error creating services."}"
                };

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
                DeleteServiceModelValidator deleteServiceModelValidation = new();
                var validationResult = deleteServiceModelValidation.Validate(deleteServiceModel);

                if (!validationResult.isSuccess)
                {
                    return validationResult;
                }

                var deleteServiceResponse = await _httpClient.PostAsync<DeleteServiceResponse>("Service/DisableService", deleteServiceModel);

                return deleteServiceResponse ?? new DeleteServiceResponse
                {
                    isSuccess = false,
                    message = deleteServiceResponse?.message ?? "Error while deleting service."
                };

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred deleting service");
                return new DeleteServiceResponse { isSuccess = false, message = "Error while deleting service" };
            }
        }


    }
}


