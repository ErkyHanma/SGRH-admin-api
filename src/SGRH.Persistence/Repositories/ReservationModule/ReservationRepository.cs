using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Dtos.ReservationModule.Reservation.Validators;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.ReservationModule
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly string? _connectionString;
        private readonly IAppLogger<ReservationRepository> _logger;
        private readonly IConfiguration _configuration;

        public ReservationRepository(IAppLogger<ReservationRepository> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:SGRHConnection"]; ;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ReservationDto>>> GetAllAsync()
        {
            try
            {
                _logger.Info("Getting all Reservations");


                var reservations = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM reservationModule.GetAllReservationsWithServices()",
                    reader => new ReservationDto
                    {
                        ReservationId = reader.GetInt32(0),
                        ClientId = reader.GetInt32(1),
                        RoomId = reader.GetInt32(2),
                        StartDate = reader.GetDateTime(3),
                        EndDate = reader.GetDateTime(4),
                        ReservationDate = reader.GetDateTime(5),
                        Status = reader.GetString(6),
                        GuestCount = reader.GetInt32(7),
                        PaymentAmount = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                        ServicesCount = reader.GetInt32(9),
                        TotalServicesCost = reader.GetDecimal(10),
                        ServiceNames = reader.IsDBNull(11) ? "" : reader.GetString(11),
                        CreatedAt = reader.GetDateTime(12),
                        UpdatedAt = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                    });

                if (reservations.Count == 0)
                {
                    return OperationResult<IEnumerable<ReservationDto>>.Failure("No reservations found");
                }

                return OperationResult<IEnumerable<ReservationDto>>.Success("Reservations retrieved successfully", reservations);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving reservations");
                return OperationResult<IEnumerable<ReservationDto>>.Failure($"Error retrieving reservations {ex.Message}");
            }
        }
        public async Task<OperationResult<ReservationDto>> GetByIdAsync(int id)
        {
            try
            {
                _logger.Info($"Getting reservation {id}");

                if (id <= 0)
                {
                    _logger.ErrorNoEx($"Trying to find reservation with invalid id {id}.");
                    return OperationResult<ReservationDto>.Failure("Invalid reservation ID");
                }


                var reservation = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM reservationModule.GetReservationByID(@p_reservationId)",
                    reader => new ReservationDto
                    {
                        ReservationId = reader.GetInt32(reader.GetOrdinal("reservation_id")),
                        ClientId = reader.GetInt32(reader.GetOrdinal("client_id")),
                        RoomId = reader.GetInt32(reader.GetOrdinal("room_id")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
                        ReservationDate = reader.GetDateTime(reader.GetOrdinal("reservation_date")),
                        Status = reader.GetString(reader.GetOrdinal("status")),
                        GuestCount = reader.GetInt32(reader.GetOrdinal("guest_count")),
                        PaymentAmount = reader.IsDBNull(reader.GetOrdinal("payment_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("payment_amount")),
                        ServicesCount = reader.GetInt32(reader.GetOrdinal("services_count")),
                        TotalServicesCost = reader.GetDecimal(reader.GetOrdinal("total_services_cost")),
                        ServiceNames = reader.IsDBNull(reader.GetOrdinal("service_names")) ? "" : reader.GetString(reader.GetOrdinal("service_names")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at")),

                    },
                    new Dictionary<string, object>
                    {
                        {"@p_reservationId", id}
                    }
                    );

                if (!reservation.Any())
                {
                    return OperationResult<ReservationDto>.Failure("Reservation not found");
                }
                return OperationResult<ReservationDto>.Success("Reservation retrieved successfully", reservation.First());

            }
            catch (Exception e)
            {
                _logger.ErrorEx(e, "Error while retrieving reservation");
                return OperationResult<ReservationDto>.Failure($"Unable to retrieve data {e.Message}");
            }
        }
        public async Task<OperationResult<CreateReservationDto>> AddAsync(CreateReservationDto createReservationDto)
        {

            _logger.Info($"Creating Reservation for client {createReservationDto.ClientId}");

            // Validation
            var createReservationDtoValidator = new CreateReservationDtoValidator();
            var validationResult = createReservationDtoValidator.Validate(createReservationDto);

            if (!validationResult.IsSuccess)
            {
                _logger.ErrorNoEx($"Validation failed for CreateReservationDto");
                return validationResult;
            }

            var parameters = new Dictionary<string, object>
            {
                {"p_client_id", createReservationDto.ClientId },
                { "p_room_id", createReservationDto.RoomId },
                { "p_start_date", createReservationDto.StartDate.Date },
                { "p_end_date", createReservationDto.EndDate.Date },
                { "p_status", createReservationDto.Status },
                { "p_guest_count",createReservationDto.GuestCount },
                { "p_payment_amount",createReservationDto.PaymentAmount },
                { "p_created_by", createReservationDto.CreatedBy },
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
                Console.WriteLine("Fake");
                return OperationResult<CreateReservationDto>.Failure(StoredProcedureResult.Message);
            }
        }
        public async Task<OperationResult<UpdateReservationDto>> UpdateAsync(UpdateReservationDto updateReservationDto)
        {
            _logger.Info($"Update reservation {updateReservationDto.ReservationId}");

            // Validation
            var updateReservationDtoValidator = new UpdateReservationDtoValidator();
            var validationResult = updateReservationDtoValidator.Validate(updateReservationDto);

            if (!validationResult.IsSuccess)
            {
                _logger.ErrorNoEx($"Validation failed for UpdateReservationDto");
                return validationResult;
            }

            var parameters = new Dictionary<string, object>()
            {
                { "p_reservation_id", updateReservationDto.ReservationId},
                { "p_client_id", updateReservationDto.ClientId},
                { "p_room_id", updateReservationDto.RoomId},
                { "p_start_date", updateReservationDto.StartDate.Date},
                { "p_end_date", updateReservationDto.EndDate.Date},
                { "p_status", updateReservationDto.Status},
                { "p_guest_count", updateReservationDto.GuestCount},
                { "p_payment_amount", updateReservationDto.PaymentAmount},
                { "p_updated_by", updateReservationDto.UpdatedBy},
            };

            var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync(
             _connectionString,
             "reservationModule.UpdateReservation",
             parameters,
             _logger
            );

            if (StoredProcedureResult.IsSuccess)
            {
                return OperationResult<UpdateReservationDto>.Success(StoredProcedureResult.Message, updateReservationDto);
            }
            else
            {
                return OperationResult<UpdateReservationDto>.Failure(StoredProcedureResult.Message);
            }
        }
        public async Task<OperationResult<DisableReservationDto>> DeleteAsync(DisableReservationDto disableReservationDto)
        {
            _logger.Info($"Disable reservation {disableReservationDto.ReservationId}");

            // Validation
            var validationResult = DisableReservationDtoValidator.Validate(disableReservationDto);

            if (!validationResult.IsSuccess)
            {
                _logger.ErrorNoEx($"Validation failed for DisableReservationDto");
                return validationResult;
            }

            var parameters = new Dictionary<string, object>()
            {
                {"p_reservation_id", disableReservationDto.ReservationId},
                {"p_updated_by", disableReservationDto.UpdatedBy},
                {"p_deleted_by", disableReservationDto.DeleteBy},
            };

            var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "reservationModule.DisableReservation",
                parameters, _logger);

            if (StoredProcedureResult.IsSuccess)
            {
                return OperationResult<DisableReservationDto>.Success(StoredProcedureResult.Message, disableReservationDto);

            }
            else
            {
                return OperationResult<DisableReservationDto>.Failure(StoredProcedureResult.Message);
            }


        }
        public async Task<OperationResult<CheckRoomAvailabilityResultDto>> CheckAvailability(int roomId, DateTime startDate, DateTime endDate)
        {

            if (roomId <= 0)
            {
                _logger.ErrorNoEx($"Invalid room ID: {roomId}");
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure("Room ID must be greater than zero.");
            }

            if (startDate >= endDate)
            {
                _logger.ErrorNoEx($"Invalid date range: startDate={startDate}, endDate={endDate}");
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure("Start date must be before end date.");
            }

            try
            {
                _logger.Info($"Checking availability for room {roomId} between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}");

                var result = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM reservationModule.CheckRoomAvailability(@p_room_id, @p_start_date, @p_end_date)",
                    reader => new CheckRoomAvailabilityResultDto
                    {
                        IsAvailable = reader.GetBoolean(reader.GetOrdinal("is_available")),
                        Message = reader.GetString(reader.GetOrdinal("f_message"))
                    },
                    new Dictionary<string, object>
                    {
                { "@p_room_id", roomId },
                { "@p_start_date", startDate },
                { "@p_end_date", endDate }
                    }
                );

                if (!result.Any())
                {
                    return OperationResult<CheckRoomAvailabilityResultDto>.Failure($"Room {roomId} not found or could not check availability.");
                }

                var availability = result.First();
                return OperationResult<CheckRoomAvailabilityResultDto>.Success(availability.Message, availability);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, $"Error checking availability for room {roomId}");
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure("An error occurred while checking room availability.");
            }
        }

    }
}
