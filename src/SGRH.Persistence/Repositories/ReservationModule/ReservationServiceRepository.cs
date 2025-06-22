using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using SGRH.Persistence.Common.Loggers.Interfaces;
using SGRH.Persistence.Helpers;


namespace SGRH.Persistence.Repositories.ReservationModule
{
    public class ReservationServiceRepository : IReservationServiceRepository
    {

        private readonly string _connectionString;
        private readonly IAppLogger<ReservationServiceRepository> _logger;

        public ReservationServiceRepository(string connectionString, IAppLogger<ReservationServiceRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<OperationResult<CreateReservationServiceDto>> AddAsync(CreateReservationServiceDto createReservationServiceDto)
        {
            _logger.Info("Add service for a reservation");

            var parameters = new Dictionary<string, object>()
            {
                {"p_reservation_id", createReservationServiceDto.ReservationId },
                {"p_service_id", createReservationServiceDto.ServiceId }
            };

            var StoreProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "reservationModule.AddServiceReservation",
                parameters,
                _logger);

            if (StoreProcedureResult.IsSuccess)
            {
                return OperationResult<CreateReservationServiceDto>.Success(StoreProcedureResult.Message, createReservationServiceDto);
            }
            else
            {
                return OperationResult<CreateReservationServiceDto>.Failure(StoreProcedureResult.Message);
            }

        }

        public async Task<OperationResult<DeleteReservationServiceDto>> DeleteAsync(DeleteReservationServiceDto deleteReservationServiceDto)
        {
            _logger.Info("Remove service for a reservation");

            var parameters = new Dictionary<string, object>()
            {
                {"p_reservation_id", deleteReservationServiceDto.ReservationId },
                {"p_service_id", deleteReservationServiceDto.ServiceId }
            };

            var StoreProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "reservationModule.DeleteServiceReservation",
                parameters,
                _logger);

            if (StoreProcedureResult.IsSuccess)
            {
                return OperationResult<DeleteReservationServiceDto>.Success(StoreProcedureResult.Message, deleteReservationServiceDto);
            }
            else
            {
                return OperationResult<DeleteReservationServiceDto>.Failure(StoreProcedureResult.Message);
            }

        }
    }
}
