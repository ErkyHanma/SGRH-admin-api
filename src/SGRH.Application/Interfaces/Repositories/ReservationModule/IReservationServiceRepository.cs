using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.ReservationModule
{
    public interface IReservationServiceRepository
    {
        public Task<OperationResult> AddAsync(CreateReservationServiceDto createReservationServiceDto);
        public Task<OperationResult> DeleteAsync(DeleteReservationServiceDto deleteReservationServiceDto);

    }
}
