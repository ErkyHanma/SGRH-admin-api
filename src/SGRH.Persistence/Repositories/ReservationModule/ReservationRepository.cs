using Microsoft.Extensions.Logging;
using Npgsql;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using System.Data;

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

            var pResult = new OperationResult<CreateReservationDto>();

            try
            {
                _logger.LogInformation($"Creating reservation number {createReservationDto.ClientId}");

                using (var context = new NpgsqlConnection(_connectionString))
                {
                    using (var command = new NpgsqlCommand("reservationModule.CreateReservation", context))
                    {

                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_client_id", createReservationDto.ClientId);
                        command.Parameters.AddWithValue("p_room_id", createReservationDto.RoomId);
                        command.Parameters.AddWithValue("p_start_date", createReservationDto.StartDate);
                        command.Parameters.AddWithValue("p_end_date", createReservationDto.EndDate);
                        command.Parameters.AddWithValue("p_status", createReservationDto.Status);
                        command.Parameters.AddWithValue("p_guest_count", createReservationDto.GuestCount);
                        command.Parameters.AddWithValue("p_created_by", createReservationDto.CreatedBy);


                        var outputParam = new NpgsqlParameter("presult", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputParam);

                        await context.OpenAsync();
                        var result = await command.ExecuteNonQueryAsync();

                        string message = outputParam.Value?.ToString() ?? "No message returned.";


                        if (result > 0)
                        {
                            pResult = OperationResult<CreateReservationDto>.Success(message, createReservationDto);
                            _logger.LogInformation($"Reservation added successfully", createReservationDto);
                        }
                        else
                        {
                            pResult = OperationResult<CreateReservationDto>.Failure(message);
                            _logger.LogError($"Failure to add reservation");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding the reservation");
                return OperationResult<CreateReservationDto>.Failure("An error ocurred while adding reservation");

            }
            return pResult;


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
