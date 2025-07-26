using SGRH.Web.Interfaces.HttpClients.ServiceModule;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;
using System.Text.Json;

namespace SGRH.Web.Services.HttpClients.ServiceModule
{
    public class ServiceHttpClient : IServiceHttpClient
    {

        private readonly HttpClient _client;

        public ServiceHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<GetAllServicesResponse> GetAllServicesAsync()
        {
            GetAllServicesResponse getAllServiceResponse = null;

            try
            {
                var response = await _client.GetAsync("Service");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    getAllServiceResponse = JsonSerializer.Deserialize<GetAllServicesResponse>(responseString);

                }
                else
                {
                    getAllServiceResponse = new GetAllServicesResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving data."
                    };
                }
            }

            catch (Exception ex)
            {
                getAllServiceResponse = new GetAllServicesResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };
            }

            return getAllServiceResponse;

        }
        public async Task<GetServiceByIdResponse> GetServiceByIdAsync(int id)
        {
            GetServiceByIdResponse getServiceByIdResponse = null;

            try
            {
                var response = await _client.GetAsync($"Service/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    getServiceByIdResponse = JsonSerializer.Deserialize<GetServiceByIdResponse>(responseString);
                }
                else
                {
                    getServiceByIdResponse = new GetServiceByIdResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving data"
                    };
                }
            }
            catch (Exception ex)
            {
                getServiceByIdResponse = new GetServiceByIdResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data"
                };
            }

            return getServiceByIdResponse;
        }
        public async Task<CreateServiceResponse> CreateServiceAsync(CreateServiceModel createServiceModel)
        {

            CreateServiceResponse createServiceResponse = null;

            try
            {
                var response = await _client.PostAsJsonAsync("Service/CreateService", createServiceModel);
                var responseContent = await response.Content.ReadAsStringAsync();

                createServiceResponse = JsonSerializer.Deserialize<CreateServiceResponse>(responseContent);


            }
            catch (Exception ex)
            {
                createServiceResponse = new CreateServiceResponse
                {
                    isSuccess = false,
                    message = "Error while creating service"
                };
            }
            return createServiceResponse;
        }
        public async Task<EditServiceResponse> GetEditServiceByIdAsync(int id)
        {
            EditServiceResponse editServiceResponse = null;

            try
            {
                var response = await _client.GetAsync($"Service/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    editServiceResponse = JsonSerializer.Deserialize<EditServiceResponse>(responseString);
                }
                else
                {
                    editServiceResponse = new EditServiceResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving data"
                    };
                }
            }
            catch (Exception ex)
            {
                editServiceResponse = new EditServiceResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data"
                };
            }

            return editServiceResponse;
        }
        public async Task<EditServiceResponse> EditServiceAsync(ServiceModel serviceModel)
        {
            EditServiceResponse editServiceResponse = null;

            try
            {
                var response = await _client.PostAsJsonAsync($"Service/UpdateService", serviceModel);

                var responseContent = await response.Content.ReadAsStringAsync();

                editServiceResponse = JsonSerializer.Deserialize<EditServiceResponse>(responseContent);

            }
            catch (Exception ex)
            {
                editServiceResponse = new EditServiceResponse
                {
                    isSuccess = false,
                    message = "Error while creating service"
                };
            }
            return editServiceResponse;
        }
        public async Task<DeleteServiceResponse> GetDeleteServiceByIdAsync(int id)
        {
            DeleteServiceResponse deleteServiceResponse = null;

            try
            {
                var response = await _client.GetAsync($"Service/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    deleteServiceResponse = JsonSerializer.Deserialize<DeleteServiceResponse>(responseString);
                }
                else
                {
                    deleteServiceResponse = new DeleteServiceResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving data"
                    };
                }
            }
            catch (Exception ex)
            {
                deleteServiceResponse = new DeleteServiceResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data"
                };
            }

            return deleteServiceResponse;
        }
        public async Task<DeleteServiceResponse> DeleteServiceAsync(DeleteServiceModel deleteServiceModel)
        {
            DeleteServiceResponse deleteServiceResponse = null;

            try
            {
                var response = await _client.PostAsJsonAsync($"Service/DisableService", deleteServiceModel);

                var responseContent = await response.Content.ReadAsStringAsync();

                deleteServiceResponse = JsonSerializer.Deserialize<DeleteServiceResponse>(responseContent);

            }
            catch (Exception ex)
            {
                deleteServiceResponse = new DeleteServiceResponse
                {
                    isSuccess = false,
                    message = "Error while creating service"
                };
            }
            return deleteServiceResponse;
        }


    }
}


