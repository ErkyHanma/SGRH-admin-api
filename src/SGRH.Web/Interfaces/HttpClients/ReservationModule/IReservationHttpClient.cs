using SGRH.Web.Models;
using SGRH.Web.Models.ReservationModule.Reservation;
using SGRH.Web.Models.ReservationModule.Reservation.Response;

namespace SGRH.Web.Interfaces.HttpClients.ReservationModule
{
    public interface IReservationHttpClient
    {
        public Task<GetAllReservationResponse> GetAllReservationAsync();
        public Task<BaseResponse<TModel>> GetReservationByIdAsync<TModel>(int id);
        public Task<CreateReservationResponse> CreateReservationAsync(CreateReservationModel createReservationModel);
        public Task<EditReservationResponse> EditReservationAsync(EditReservationModel editReservationModel);
        public Task<DeleteReservationResponse> DeleteReservationAsync(DeleteReservationModel deleteReservationModel);
    }
}
