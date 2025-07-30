using SGRH.Application.Common.Logging;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ReservationModule.ReservationService.Response;
using SGRH.Web.Models.ServiceModule.Response;
using System.Text.Json;

namespace SGRH.Web.Services.HttpClients.ReservationModule
{
    public class ReservationServiceHttpClient : IReservationServiceHttpClient
    {

        private readonly HttpClient _client;
        private readonly IAppLogger<ReservationServiceHttpClient> _logger;


        public ReservationServiceHttpClient(HttpClient client, IAppLogger<ReservationServiceHttpClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<GetAllServicesResponse> GetReservationServicesAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"Service/{id}");
                var responseString = await response.Content.ReadAsStringAsync();
                var addReservationServiceResponse = JsonSerializer.Deserialize<GetAllServicesResponse>(responseString);

                if (response.IsSuccessStatusCode && addReservationServiceResponse is not null)
                {
                    return addReservationServiceResponse;
                }
                else
                {
                    return new GetAllServicesResponse { isSuccess = false, message = addReservationServiceResponse?.message ?? $"Error retrieving services." };
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while retrieving services.");
                return new GetAllServicesResponse { isSuccess = false, message = $"Error retrieving services. {ex.Message}" };

            }
        }

        public async Task<AddReservationServiceResponse> AddReservationAsync(AddReservationServiceModel addReservationServiceModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync("ReservationService/AddReservationService", addReservationServiceModel);
                var responseString = await response.Content.ReadAsStringAsync();
                var addReservationServiceResponse = JsonSerializer.Deserialize<AddReservationServiceResponse>(responseString);

                if (response.IsSuccessStatusCode && addReservationServiceResponse is not null)
                {
                    return addReservationServiceResponse;
                }
                else
                {
                    return new AddReservationServiceResponse { isSuccess = false, message = addReservationServiceResponse?.message ?? $"Error adding service." };
                }

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while adding service.");
                return new AddReservationServiceResponse { isSuccess = false, message = $"Error adding service. {ex.Message}" };
            }
        }

        public async Task<DeleteReservationServiceResponse> DeleteReservationAsync(DeleteReservationServiceModel deleteReservationServiceModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync("ReservationService/DeleteReservationService", deleteReservationServiceModel);
                var responseString = await response.Content.ReadAsStringAsync();
                var deleteServiceResponse = JsonSerializer.Deserialize<DeleteReservationServiceResponse>(responseString);

                if (response.IsSuccessStatusCode && deleteServiceResponse is not null)
                {
                    return deleteServiceResponse;
                }
                else
                {
                    return new DeleteReservationServiceResponse { isSuccess = false, message = deleteServiceResponse?.message ?? $"Error removing service." };
                }


            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while removing service.");
                return new DeleteReservationServiceResponse { isSuccess = false, message = $"Error removing service. {ex.Message}" };

            }

        }



    }
}
