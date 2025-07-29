using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ReservationModule.ReservationService.Response;

namespace SGRH.Web.Interfaces.HttpClients.ReservationModule
{
    public interface IReservationServiceHttpClient
    {
        public Task<AddReservationServiceResponse> AddReservationAsync(AddReservationServiceModel addReservationServiceModel);
        public Task<DeleteReservationServiceResponse> DeleteReservationAsync(DeleteReservationServiceModel deleteReservationServiceModel);

    }
}
