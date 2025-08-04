using Microsoft.Extensions.Configuration;
using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Common.Common;
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

            if (createReservationServiceDto == null)
            {
                _logger.ErrorNoEx("Error adding reservation service. Dto Null");
                return OperationResult<CreateReservationServiceDto>.Failure("Dto cannot be null.");
            }

            _logger.Info("Adding Reservation service", createReservationServiceDto);

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

            if (deleteReservationServiceDto == null)
            {
                _logger.ErrorNoEx("Error removing reservation service. Dto Null");
                return OperationResult<DeleteReservationServiceDto>.Failure("Dto cannot be null.");
            }


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

        public async Task<OperationResult<bool>> IsServiceAdded(int reservationID, int serviceId)
        {
            if (reservationID <= 0)
            {
                _logger.ErrorNoEx($"Invalid reservation ID: {reservationID}");
                return OperationResult<bool>.Failure("Invalid reservation ID: it must be greater than zero.");
            }

            if (serviceId <= 0)
            {
                _logger.ErrorNoEx($"Invalid service ID: {serviceId}");
                return OperationResult<bool>.Failure("Invalid service ID: it must be greater than zero.");
            }

            try
            {
                _logger.Info($"Checking if service {serviceId} is added to reservation {reservationID}");

                var result = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    """
                    SELECT 1 AS is_present
                    FROM reservationModule.reservation_service
                    WHERE reservation_id = @p_reservation_id
                      AND service_id = @p_service_id
                      AND is_deleted = FALSE
                    """,
                    reader => reader.GetInt32(reader.GetOrdinal("is_present")),
                    new Dictionary<string, object>
                    {
                { "@p_reservation_id", reservationID },
                { "@p_service_id", serviceId }
                    }
                );

                bool exists = result.Any();


                if (!exists)
                {
                    return OperationResult<bool>.Success("Service is not added to reservation.", false);
                }

                return OperationResult<bool>.Success("Service is already added to reservation.", true);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, $"Error checking if service {serviceId} is added to reservation {reservationID}");
                return OperationResult<bool>.Failure("An error occurred while checking if the service is added.");
            }
        }

    }
}


