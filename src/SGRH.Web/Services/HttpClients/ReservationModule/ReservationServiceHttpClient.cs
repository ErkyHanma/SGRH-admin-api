using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ReservationModule.ReservationService.Response;
using System.Text.Json;

namespace SGRH.Web.Services.HttpClients.ReservationModule
{
    public class ReservationServiceHttpClient : IReservationServiceHttpClient
    {

        private readonly HttpClient _client;


        public ReservationServiceHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<AddReservationServiceResponse> AddReservationAsync(AddReservationServiceModel addReservationServiceModel)
        {
            AddReservationServiceResponse? addReservationServiceResponse;

            try
            {
                var response = await _client.PostAsJsonAsync("ReservationService/AddReservationService", addReservationServiceModel);
                var responseString = await response.Content.ReadAsStringAsync();

                addReservationServiceResponse = JsonSerializer.Deserialize<AddReservationServiceResponse>(responseString);
            }
            catch (Exception ex)
            {
                addReservationServiceResponse = new AddReservationServiceResponse
                {
                    isSuccess = false,
                    message = $"Error retriving data. {ex.Message}"
                };

            }
            return addReservationServiceResponse;
        }

        public async Task<DeleteReservationServiceResponse> DeleteReservationAsync(DeleteReservationServiceModel deleteReservationServiceModel)
        {
            DeleteReservationServiceResponse? deleteReservationServiceResponse;

            try
            {
                var response = await _client.PostAsJsonAsync("ReservationService/DeleteReservationService", deleteReservationServiceModel);
                var responseString = await response.Content.ReadAsStringAsync();

                deleteReservationServiceResponse = JsonSerializer.Deserialize<DeleteReservationServiceResponse>(responseString);
            }
            catch (Exception ex)
            {
                deleteReservationServiceResponse = new DeleteReservationServiceResponse
                {
                    isSuccess = false,
                    message = $"Error retriving data. {ex.Message}"
                };

            }
            return deleteReservationServiceResponse;
        }
    }
}
