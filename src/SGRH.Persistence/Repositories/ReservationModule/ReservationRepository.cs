using Microsoft.Extensions.Logging;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;

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

            throw new NotImplementedException();

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
