using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.ReservationModule
{
    public interface IReservationRepository
    {
        Task<OperationResult<IEnumerable<ReservationDto>>> GetAllAsync();
        Task<OperationResult<ReservationDto>> GetByIdAsync(int id);
        Task<OperationResult<CreateReservationDto>> AddAsync(CreateReservationDto createReservationDto);
        Task<OperationResult<UpdateReservationDto>> UpdateAsync(UpdateReservationDto updateReservationDto);
        Task<OperationResult<DisableReservationDto>> DeleteAsync(DisableReservationDto disableReservationDto);
        Task<OperationResult<CheckRoomAvailabilityResultDto>> CheckAvailability(CheckRoomAvailabilityResultDto checkRoomAvailabilityResultDto);
    }
}
