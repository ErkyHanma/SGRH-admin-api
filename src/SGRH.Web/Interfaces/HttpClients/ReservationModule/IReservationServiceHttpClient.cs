using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ReservationModule.ReservationService.Response;
using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Interfaces.HttpClients.ReservationModule
{
    public interface IReservationServiceHttpClient
    {

        public Task<GetAllServicesResponse> GetReservationServicesAsync(int id);
        public Task<AddReservationServiceResponse> AddReservationAsync(AddReservationServiceModel addReservationServiceModel);
        public Task<DeleteReservationServiceResponse> DeleteReservationAsync(DeleteReservationServiceModel deleteReservationServiceModel);

    }
}
