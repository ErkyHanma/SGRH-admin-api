using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Services.ReservationModule
{
    public interface IReservationService
    {
        Task<OperationResult<IEnumerable<ReservationDto>>> GetAllReservationAsync();
        Task<OperationResult<ReservationDto>> GetReservationByIdAsync(int id);
        Task<OperationResult<CreateReservationDto>> AddReservationAsync(CreateReservationDto createReservationDto);
        Task<OperationResult<UpdateReservationDto>> UpdateReservationAsync(UpdateReservationDto updateReservationDto);
        Task<OperationResult<DeleteReservationDto>> DeleteReservationAsync(DeleteReservationDto disableReservationDto);
        Task<OperationResult<CheckRoomAvailabilityResultDto>> CheckAvailability(int RoomId, DateTime StartDate, DateTime EndDate);
    }
}
