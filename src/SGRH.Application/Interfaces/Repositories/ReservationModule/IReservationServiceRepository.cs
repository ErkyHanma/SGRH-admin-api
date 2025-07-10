using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.ReservationModule
{
    public interface IReservationServiceRepository
    {
        public Task<OperationResult<CreateReservationServiceDto>> AddAsync(CreateReservationServiceDto createReservationServiceDto);
        public Task<OperationResult<DeleteReservationServiceDto>> DeleteAsync(DeleteReservationServiceDto deleteReservationServiceDto);

        public Task<OperationResult<bool>> IsServiceAdded(int reservationID, int serviceId);

    }
}
