using Microsoft.Extensions.Logging;
using Npgsql;
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
            var reservations = new List<ReservationDto>();
            try
            {
                _logger.LogInformation("Getting all Reservations");

                using (var context = new NpgsqlConnection(_connectionString))
                {
                    await context.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT * FROM reservationModule.GetAllReservationsWithServices", context))
                    {
                        using var reader = await command.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            var reservation = new ReservationDto
                            {
                                ReservationId = reader.GetInt32(0),
                                ClientId = reader.GetInt32(1),
                                RoomId = reader.GetInt32(2),
                                StartDate = reader.GetDateTime(3),
                                EndDate = reader.GetDateTime(4),
                                ReservationDate = reader.GetDateTime(5),
                                Status = reader.GetString(6),
                                GuestCount = reader.GetInt32(7),
                                PaymentAmount = reader.GetDecimal(8),
                                ServicesCount = reader.GetInt32(9),
                                TotalServicesCost = reader.GetDecimal(10),
                                ServiceNames = reader.IsDBNull(11) ? "" : reader.GetString(11),
                                CreatedAt = reader.GetDateTime(12),
                                UpdatedAt = reader.GetDateTime(13)
                            };

                            reservations.Add(reservation);
                        }
                    }
                }

                if (reservations.Count == 0)
                {
                    return OperationResult<IEnumerable<ReservationDto>>.Failure("No reservations found");
                }

                return OperationResult<IEnumerable<ReservationDto>>.Success("Reservations retrieved successfully", reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reservations");
                return OperationResult<IEnumerable<ReservationDto>>.Failure("Error retrieving reservations");
            }
        }
        public async Task<OperationResult<ReservationDto>> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Getting reservation {id}");

                using (var context = new NpgsqlConnection(_connectionString))
                {
                    await context.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT * FROM reservationModule.GetReservationByID(@p_reservationId)", context))
                    {
                        command.Parameters.AddWithValue("@p_reservationId", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var reservation = new ReservationDto
                                {
                                    ReservationId = reader.GetInt32(reader.GetOrdinal("reservation_id")),
                                    ClientId = reader.GetInt32(reader.GetOrdinal("client_id")),
                                    RoomId = reader.GetInt32(reader.GetOrdinal("room_id")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
                                    ReservationDate = reader.GetDateTime(reader.GetOrdinal("reservation_date")),
                                    Status = reader.GetString(reader.GetOrdinal("status")),
                                    GuestCount = reader.GetInt32(reader.GetOrdinal("guest_count")),
                                    PaymentAmount = reader.GetDecimal(reader.GetOrdinal("payment_amount")),
                                    ServicesCount = reader.GetInt32(reader.GetOrdinal("services_count")),
                                    TotalServicesCost = reader.GetDecimal(reader.GetOrdinal("total_services_cost")),
                                    ServiceNames = reader.IsDBNull(reader.GetOrdinal("service_names")) ? "" : reader.GetString(reader.GetOrdinal("service_names")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),

                                };



                                return OperationResult<ReservationDto>.Success("Reservation retrieved successfully", reservation);
                            }
                        }
                    }
                }

                return OperationResult<ReservationDto>.Failure("Reservation not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving reservation");
                return OperationResult<ReservationDto>.Failure("Unable to retrieve data");
            }
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
            _logger.LogInformation($"Update reservation {updateReservationDto.ReservationId}");

            var parameters = new Dictionary<string, object>()
            {
                { "p_reservation_id", updateReservationDto.ReservationId},
                { "p_client_id", updateReservationDto.ReservationId},
                { "p_room_id", updateReservationDto.ReservationId},
                { "p_start_date", updateReservationDto.ReservationId},
                { "p_end_date", updateReservationDto.ReservationId},
                { "p_status", updateReservationDto.ReservationId},
                { "p_guest_count", updateReservationDto.ReservationId},
                { "p_updated_by", updateReservationDto.ReservationId},
            };

            var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync(
             _connectionString,
             "reservationModule.CreateReservation",
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
            _logger.LogInformation($"Disable reservation {disableReservationDto.ReservationId}");

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
        public async Task<OperationResult<CheckRoomAvailabilityResultDto>> CheckAvailability(int RoomId, DateTime StartDate, DateTime EndDate)
        {

            try
            {
                _logger.LogInformation($"Checking room {RoomId}");

                using (var context = new NpgsqlConnection(_connectionString))
                {
                    await context.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT * from reservationModule.CheckRoomAvailability(@p_room_id, @p_start_date, @p_end_date)", context))
                    {
                        command.Parameters.AddWithValue("@p_room_id", RoomId);
                        command.Parameters.AddWithValue("@p_start_date", StartDate);
                        command.Parameters.AddWithValue("@p_end_date", EndDate);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var availability = new CheckRoomAvailabilityResultDto
                                {
                                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("is_available")),
                                    Message = reader.GetString(reader.GetOrdinal("f_message"))
                                };

                                return OperationResult<CheckRoomAvailabilityResultDto>.Success(availability.Message, availability);
                            }
                        }
                    }
                }
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure($"The room {RoomId} was not found ");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving data");
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure("Unable to retrieve data");
            }


        }
    }
}
