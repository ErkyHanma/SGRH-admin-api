using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.ReservationModule
{
    public interface IReservationRepository
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(CreateReservationDto createReservationDto);
        Task<OperationResult> UpdateAsync(UpdateReservationDto updateReservationDto);
        Task<OperationResult> DeleteAsync(DisableReservationDto disableReservationDto);
        Task<OperationResult> CheckAvailability(CheckRoomAvailabilityResultDto checkRoomAvailabilityResultDto);
    }
}
