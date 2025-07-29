using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models.ReservationModule.Reservation;
using SGRH.Web.Models.ReservationModule.Reservation.Response;
using System.Text.Json;

namespace SGRH.Web.Services.HttpClients.ReservationModule
{
    public class ReservationHttpClient : IReservationHttpClient
    {
        private readonly HttpClient _client;

        public ReservationHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<GetAllReservationResponse> GetAllReservationAsync()
        {

            GetAllReservationResponse getAllReservationResponse = null;
            try
            {
                var response = await _client.GetAsync("Reservation");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    getAllReservationResponse = JsonSerializer.Deserialize<GetAllReservationResponse>(responseString);
                }
                else
                {
                    getAllReservationResponse = new GetAllReservationResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving reservations"
                    };
                }
            }
            catch (Exception ex)
            {
                getAllReservationResponse = new GetAllReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving reservations. {ex.Message}"
                };
            }

            return getAllReservationResponse;
        }
        public async Task<GetReservationByIdResponse> GetReservationByIdAsync(int id)
        {
            GetReservationByIdResponse getReservationByIdResponse = null;
            try
            {
                var response = await _client.GetAsync($"Reservation/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    getReservationByIdResponse = JsonSerializer.Deserialize<GetReservationByIdResponse>(responseString);
                }
                else
                {
                    getReservationByIdResponse = new GetReservationByIdResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving reservations"
                    };
                }
            }
            catch (Exception ex)
            {
                getReservationByIdResponse = new GetReservationByIdResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving reservations. {ex.Message}"
                };
            }

            return getReservationByIdResponse;
        }
        public async Task<CreateReservationResponse> CreateReservationAsync(CreateReservationModel createReservationModel)
        {

            CreateReservationResponse createReservationResponse = null;

            try
            {
                var response = await _client.PostAsJsonAsync("Reservation/CreateReservation", createReservationModel);

                var responseString = await response.Content.ReadAsStringAsync();

                createReservationResponse = JsonSerializer.Deserialize<CreateReservationResponse>(responseString);
            }
            catch (Exception ex)
            {
                createReservationResponse = new CreateReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving data. {ex.Message}"
                };
            }

            return createReservationResponse;
        }
        public async Task<EditReservationResponse> GetEditReservationByIdAsync(int id)
        {
            EditReservationResponse editReservationResponse = null;
            try
            {
                var response = await _client.GetAsync($"Reservation/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    editReservationResponse = JsonSerializer.Deserialize<EditReservationResponse>(responseString);
                }
                else
                {
                    editReservationResponse = new EditReservationResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving reservations"
                    };
                }
            }
            catch (Exception ex)
            {
                editReservationResponse = new EditReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving reservations. {ex.Message}"
                };
            }

            return editReservationResponse;
        }
        public async Task<EditReservationResponse> EditReservationAsync(EditReservationModel editReservationModel)
        {
            EditReservationResponse editReservationResponse = null;

            try
            {
                var response = await _client.PostAsJsonAsync("Reservation/UpdateReservation", editReservationModel);

                var responseString = await response.Content.ReadAsStringAsync();

                editReservationResponse = JsonSerializer.Deserialize<EditReservationResponse>(responseString);
            }
            catch (Exception ex)
            {
                editReservationResponse = new EditReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving data. {ex.Message}"
                };
            }

            return editReservationResponse;
        }
        public async Task<DeleteReservationResponse> GetDeleteReservationByIdAsync(int id)
        {
            DeleteReservationResponse deleteReservationResponse = null;
            try
            {
                var response = await _client.GetAsync($"Reservation/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    deleteReservationResponse = JsonSerializer.Deserialize<DeleteReservationResponse>(responseString);
                }
                else
                {
                    deleteReservationResponse = new DeleteReservationResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving reservations"
                    };
                }
            }
            catch (Exception ex)
            {
                deleteReservationResponse = new DeleteReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving reservations. {ex.Message}"
                };
            }

            return deleteReservationResponse;
        }
        public async Task<DeleteReservationResponse> DeleteReservationAsync(DeleteReservationModel deleteReservationModel)
        {
            DeleteReservationResponse? deleteReservationResponse;

            try
            {
                var response = await _client.PostAsJsonAsync("Reservation/DisableReservation", deleteReservationModel);

                var responseString = await response.Content.ReadAsStringAsync();

                deleteReservationResponse = JsonSerializer.Deserialize<DeleteReservationResponse>(responseString);
            }
            catch (Exception ex)
            {
                deleteReservationResponse = new DeleteReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving data. {ex.Message}"
                };
            }

            return deleteReservationResponse;
        }


    }
}
