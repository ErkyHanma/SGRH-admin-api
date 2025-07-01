using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Dtos.ReservationModule.ReservationService.Validators;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;


namespace SGRH.Persistence.Repositories.ReservationModule
{
    public class ReservationServiceRepository : IReservationServiceRepository
    {

        private readonly string? _connectionString;
        private readonly IAppLogger<ReservationServiceRepository> _logger;
        private readonly IConfiguration _configuration;

        public ReservationServiceRepository(IAppLogger<ReservationServiceRepository> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:SGRHConnection"];
            _logger = logger;
        }

        public async Task<OperationResult<CreateReservationServiceDto>> AddAsync(CreateReservationServiceDto createReservationServiceDto)
        {
            _logger.Info("Add service for a reservation");

            // Validation
            var validationResult = CreateReservationServiceDtoValidator.Validate(createReservationServiceDto);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

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

            // Validation
            var validationResult = DeleteReservationServiceDtoValidator.Validate(deleteReservationServiceDto);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

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
