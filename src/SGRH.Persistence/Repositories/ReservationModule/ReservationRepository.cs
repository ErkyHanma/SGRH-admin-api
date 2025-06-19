using Microsoft.Extensions.Logging;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.ReservationModule
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ReservationRepository> _logger;

        public ReservationRepository(string connectionString, ILogger<ReservationRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ReservationDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<OperationResult<ReservationDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<OperationResult<CreateReservationDto>> AddAsync(CreateReservationDto createReservationDto)
        {

            _logger.LogInformation($"Creating Reservation for client {createReservationDto.ClientId}");

            var parameters = new Dictionary<string, object>
            {
                {"p_client_id", createReservationDto.ClientId },
                { "p_room_id", createReservationDto.RoomId },
                { "p_start_date", createReservationDto.StartDate },
                { "p_end_date", createReservationDto.EndDate },
                { "p_status", createReservationDto.Status },
                { "p_guest_count",createReservationDto.GuestCount },
                { "p_created_by", createReservationDto.CreatedBy }
            };

            var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "reservationModule.CreateReservation",
                parameters,
                _logger
            );

            if (StoredProcedureResult.IsSuccess)
            {
                return OperationResult<CreateReservationDto>.Success(StoredProcedureResult.Message, createReservationDto);
            }
            else
            {
                return OperationResult<CreateReservationDto>.Failure(StoredProcedureResult.Message);
            }
        }



        public async Task<OperationResult<UpdateReservationDto>> UpdateAsync(UpdateReservationDto updateReservationDto)
        {
            throw new NotImplementedException();
        }
        public async Task<OperationResult<DisableReservationDto>> DeleteAsync(DisableReservationDto disableReservationDto)
        {
            throw new NotImplementedException();
        }
        public async Task<OperationResult<CheckRoomAvailabilityResultDto>> CheckAvailability(CheckRoomAvailabilityResultDto checkRoomAvailabilityResultDto)
        {
            throw new NotImplementedException();
        }
    }
}
