using SGRH.Application.Common.Logging;
using SGRH.Web.Interfaces.HttpClients;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models;
using SGRH.Web.Models.ReservationModule.Reservation;
using SGRH.Web.Models.ReservationModule.Reservation.Response;

namespace SGRH.Web.Services.HttpClients.ReservationModule
{
    public class ReservationHttpClient : IReservationHttpClient
    {
        private readonly IBaseHttpClientMethods _httpClient;
        private readonly IAppLogger<ReservationHttpClient> _logger;

        public ReservationHttpClient(IBaseHttpClientMethods httpClient, IAppLogger<ReservationHttpClient> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<GetAllReservationResponse> GetAllReservationAsync()
        {

            try
            {
                var response = await _httpClient.GetAsync<GetAllReservationResponse>("Reservation");

                return response ?? new GetAllReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving reservations."
                };
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
                var response = await _httpClient.GetAsync<BaseResponse<TModel>>($"Reservation/{id}");

                return response ?? new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "Error retrieving reservation."
                };

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
                var createReservationResponse = await _httpClient.PostAsync<CreateReservationResponse>("Reservation/CreateReservation", createReservationModel);

                return createReservationResponse ?? new CreateReservationResponse
                {
                    isSuccess = false,
                    message = createReservationResponse?.message ?? "Error creating reservation"
                };

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
                var editReservationResponse = await _httpClient.PostAsync<EditReservationResponse>("Reservation/UpdateReservation", editReservationModel);

                return editReservationResponse ?? new EditReservationResponse
                {
                    isSuccess = false,
                    message = editReservationResponse?.message ?? "Error modifying reservation"
                };

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
                var deleteReservationResponse = await _httpClient.PostAsync<DeleteReservationResponse>("Reservation/DisableReservation", deleteReservationModel);

                return deleteReservationResponse ?? new DeleteReservationResponse
                {
                    isSuccess = false,
                    message = deleteReservationResponse?.message ?? "Error deleting reservation"
                };

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while deleting reservation.");
                return new DeleteReservationResponse { isSuccess = false, message = $"Error deleting reservation. {ex.Message}" };
            }

        }


    }
}


