using SGRH.Application.Common.Logging;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models;
using SGRH.Web.Models.ReservationModule.Reservation;
using SGRH.Web.Models.ReservationModule.Reservation.Response;
using System.Text.Json;

namespace SGRH.Web.Services.HttpClients.ReservationModule
{
    public class ReservationHttpClient : IReservationHttpClient
    {
        private readonly HttpClient _client;
        private readonly IAppLogger<ReservationHttpClient> _logger;

        public ReservationHttpClient(HttpClient client, IAppLogger<ReservationHttpClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<GetAllReservationResponse> GetAllReservationAsync()
        {

            try
            {
                var response = await _client.GetAsync("Reservation");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<GetAllReservationResponse>(responseString);
                }
                else
                {
                    return new GetAllReservationResponse { isSuccess = false, message = "Error retrieving reservations" };
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while retrieving reservation.");
                return new GetAllReservationResponse { isSuccess = false, message = $"Error retrieving reservations. {ex.Message}" };
            }


        }
        public async Task<BaseResponse<TModel>> GetReservationByIdAsync<TModel>(int id)
        {
            try
            {
                var response = await _client.GetAsync($"Reservation/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<BaseResponse<TModel>>(responseString);
                }
                else
                {
                    return new BaseResponse<TModel> { isSuccess = false, message = "Error retrieving reservation." };
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while retrieving reservation.");
                return new BaseResponse<TModel> { isSuccess = false, message = "Error retrieving reservation." };
            }
        }
        public async Task<CreateReservationResponse> CreateReservationAsync(CreateReservationModel createReservationModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync("Reservation/CreateReservation", createReservationModel);
                var responseString = await response.Content.ReadAsStringAsync();
                var createReservationResponse = JsonSerializer.Deserialize<CreateReservationResponse>(responseString);

                if (response.IsSuccessStatusCode && createReservationResponse is not null)
                {
                    return createReservationResponse;
                }
                else
                {
                    return new CreateReservationResponse { isSuccess = false, message = createReservationResponse?.message ?? "Error creating reservation" };
                }

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while creating reservation.");
                return new CreateReservationResponse { isSuccess = false, message = "Error creating reservation" };
            }

        }
        public async Task<EditReservationResponse> EditReservationAsync(EditReservationModel editReservationModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync("Reservation/UpdateReservation", editReservationModel);
                var responseString = await response.Content.ReadAsStringAsync();
                var editReservationResponse = JsonSerializer.Deserialize<EditReservationResponse>(responseString);

                if (response.IsSuccessStatusCode && editReservationResponse is not null)
                {
                    return editReservationResponse;
                }
                else
                {
                    return new EditReservationResponse { isSuccess = false, message = editReservationResponse?.message ?? "Error modifying reservation" };
                }

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while modifying reservation.");
                return new EditReservationResponse { isSuccess = false, message = $"Error modifying reservation. {ex.Message}" };
            }


        }
        public async Task<DeleteReservationResponse> DeleteReservationAsync(DeleteReservationModel deleteReservationModel)
        {

            try
            {
                var response = await _client.PostAsJsonAsync("Reservation/DisableReservation", deleteReservationModel);
                var responseString = await response.Content.ReadAsStringAsync();
                var deleteReservationResponse = JsonSerializer.Deserialize<DeleteReservationResponse>(responseString);

                if (response.IsSuccessStatusCode && deleteReservationResponse is not null)
                {
                    return deleteReservationResponse;
                }
                else
                {
                    return new DeleteReservationResponse { isSuccess = false, message = deleteReservationResponse?.message ?? "Error deleting reservation" };
                }


            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while deleting reservation.");
                return new DeleteReservationResponse { isSuccess = false, message = $"Error deleting reservation. {ex.Message}" };
            }

        }


    }
}


