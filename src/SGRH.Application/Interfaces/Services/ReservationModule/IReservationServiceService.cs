using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Domain.Base;


namespace SGRH.Application.Interfaces.Services.ReservationModule
{
    public interface IReservationServiceService
    {
        public Task<OperationResult<CreateReservationServiceDto>> AddReservationServiceAsync(CreateReservationServiceDto createReservationServiceDto);
        public Task<OperationResult<DeleteReservationServiceDto>> DeleteReservationServiceAsync(DeleteReservationServiceDto deleteReservationServiceDto);
    }
}
