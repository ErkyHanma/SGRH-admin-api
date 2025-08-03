using SGRH.Application.Common.Logging;
using SGRH.Web.Interfaces.HttpClients;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ReservationModule.ReservationService.Response;
using SGRH.Web.Models.ReservationModule.ReservationService.Validation;
using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Services.HttpClients.ReservationModule
{
    public class ReservationServiceHttpClient : IReservationServiceHttpClient
    {

        private readonly IBaseHttpClientMethods _httpClient;
        private readonly IAppLogger<ReservationServiceHttpClient> _logger;


        public ReservationServiceHttpClient(IBaseHttpClientMethods httpClient, IAppLogger<ReservationServiceHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<GetAllServicesResponse> GetReservationServicesAsync(int id)
        {
            try
            {
                var addReservationServiceResponse = await _httpClient.GetAsync<GetAllServicesResponse>($"Service");

                return addReservationServiceResponse ?? new GetAllServicesResponse
                {
                    isSuccess = false,
                    message = addReservationServiceResponse?.message ?? $"Error retrieving services."
                };

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
                CreateReservationServiceModelValidator createReservationServiceModelValidator = new();
                var validationResult = createReservationServiceModelValidator.Validate(addReservationServiceModel);

                if (!validationResult.isSuccess)
                {
                    return validationResult;
                }


                var addReservationServiceResponse = await _httpClient.PostAsync<AddReservationServiceResponse>("ReservationService/AddReservationService", addReservationServiceModel);

                return addReservationServiceResponse ?? new AddReservationServiceResponse
                {
                    isSuccess = false,
                    message = addReservationServiceResponse?.message ?? $"Error adding service."
                };

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
                DeleteReservationServiceModelValidator deleteReservationServiceModelValidator = new();
                var validationResult = deleteReservationServiceModelValidator.Validate(deleteReservationServiceModel);

                if (!validationResult.isSuccess)
                {
                    return validationResult;
                }

                var deleteServiceResponse = await _httpClient.PostAsync<DeleteReservationServiceResponse>("ReservationService/DeleteReservationService", deleteReservationServiceModel);

                return deleteServiceResponse ?? new DeleteReservationServiceResponse
                {
                    isSuccess = false,
                    message = deleteServiceResponse?.message ?? $"Error removing service."
                };

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "An exception occurred while removing service.");
                return new DeleteReservationServiceResponse { isSuccess = false, message = $"Error removing service. {ex.Message}" };

            }

        }



    }
}
