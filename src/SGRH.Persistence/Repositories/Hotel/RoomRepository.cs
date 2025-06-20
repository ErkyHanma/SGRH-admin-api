using FluentValidation;
using Microsoft.Extensions.Logging;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Interfaces.Repositories.Hotel; // Cuidado con esto?
using SGRH.Domain.Base; // Cuidado con esto?
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.Hotel
{
    // Logs (!)
    public class RoomRepository : IRoomRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<RoomRepository> _logger;
        private readonly IValidator<CreateRoomDto> _createValidator;
        private readonly IValidator<ModifyRoomDto> _modifyValidator;
        private readonly IValidator<DisableRoomDto> _disableValidator;

        public RoomRepository(string connetionString, ILogger<RoomRepository> logger, IValidator<CreateRoomDto> createValidator, IValidator<ModifyRoomDto> modifyValidator, IValidator<DisableRoomDto> disableValidator)
        {
            _connectionString = connetionString;
            _logger = logger;
            _createValidator = createValidator;
            _modifyValidator = modifyValidator;
            _disableValidator = disableValidator;
        }

        public async Task<OperationResult<CreateRoomDto>> AddAsync(CreateRoomDto createRoomDto)
        {
            var validationResult = _createValidator.Validate(createRoomDto);

            if (!validationResult.IsValid) // (Futura mejora?)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)); // Cadena de errores
                _logger.LogWarning("Validation failed: {Message}", message);
                return OperationResult<CreateRoomDto>.Failure(message);
            }

            _logger.LogInformation("Creating Room {RoomNumber}", createRoomDto.RoomNumber);


            var parameters = new Dictionary<string, object>
            {
                {"p_room_number", createRoomDto.RoomNumber },
                { "p_category_id", createRoomDto.CategoryId },
                { "p_floor_id", createRoomDto.FloorId },
                { "p_description", createRoomDto.Description },
                { "p_room_img_url", createRoomDto.RoomImgUrl },
                { "p_status", createRoomDto.Status },
                { "p_created_by", createRoomDto.CreatedBy }
            };

            var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "hotel.CreateRoom",
                parameters,
                _logger
            );

            if (StoredProcedureResult.IsSuccess)
            {
                return OperationResult<CreateRoomDto>.Success(StoredProcedureResult.Message, createRoomDto);
            }
            else
            {
                return OperationResult<CreateRoomDto>.Failure(StoredProcedureResult.Message);
            }
        }

        public async Task<OperationResult<DisableRoomDto>> DeleteAsync(DisableRoomDto disableRoomDto)
        {
            _logger.LogInformation("Disabling Room ID {RoomId}", disableRoomDto.RoomId);

            var parameters = new Dictionary<string, object>
            {
                { "p_room_id", disableRoomDto.RoomId },
                { "p_updated_by", disableRoomDto.UpdatedBy }
            };

            var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "hotel.DisableRoom",
                parameters,
                _logger
            );

            if (StoredProcedureResult.IsSuccess)
            {
                return OperationResult<DisableRoomDto>.Success(StoredProcedureResult.Message, disableRoomDto);
            }
            else
            {
                return OperationResult<DisableRoomDto>.Failure(StoredProcedureResult.Message);
            }
        }

        public async Task<OperationResult<IEnumerable<RoomDto>>> GetAllAsync()
        {
            try
            {
                //Captura de variables.

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetRooms()",
                    reader => new RoomDto
                    {
                        RoomId = reader.GetInt32(reader.GetOrdinal("room_id")),
                        RoomNumber = reader.GetString(reader.GetOrdinal("room_number")),
                        CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                        FloorId = reader.GetInt32(reader.GetOrdinal("floor_id")),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                        RoomImgUrl = reader.IsDBNull(reader.GetOrdinal("room_img_url")) ? null : reader.GetString(reader.GetOrdinal("room_img_url")),
                        Status = reader.GetString(reader.GetOrdinal("status")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by"))
                    });

                return OperationResult<IEnumerable<RoomDto>>.Success("Habitaciones obtenidas correctamente", data);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllAsync()");
                return OperationResult<IEnumerable<RoomDto>>.Failure("Error al obtener habitaciones.");
            }
        }

        public async Task<OperationResult<RoomDto>> GetByIdAsync(int id)
        {
            try
            {
                //Captura de variables + diccionario para buscar por ID.

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetRoomsById(@p_room_id)",
                    reader => new RoomDto
                    {
                        RoomId = reader.GetInt32(reader.GetOrdinal("room_id")),
                        RoomNumber = reader.GetString(reader.GetOrdinal("room_number")),
                        CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                        FloorId = reader.GetInt32(reader.GetOrdinal("floor_id")),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                        RoomImgUrl = reader.IsDBNull(reader.GetOrdinal("room_img_url")) ? null : reader.GetString(reader.GetOrdinal("room_img_url")),
                        Status = reader.GetString(reader.GetOrdinal("status")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by"))
                    },
                    new Dictionary<string, object>
                    {
                        { "p_room_id", id }
                    });

                //Validaciones

                if (!data.Any())
                {
                    return OperationResult<RoomDto>.Failure("Habitación no encontrada");
                }

                return OperationResult<RoomDto>.Success("Habitación obtenida correctamente", data.First());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetByIdAsync()");
                return OperationResult<RoomDto>.Failure("Error al obtener habitación.");
            }
        }

        public async Task<OperationResult<ModifyRoomDto>> UpdateAsync(ModifyRoomDto modifyRoomDto)
        {
            var validationResult = _modifyValidator.Validate(modifyRoomDto);

            if (!validationResult.IsValid)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed: {Message}", message);
                return OperationResult<ModifyRoomDto>.Failure(message);
            }

            _logger.LogInformation("Updating Room {RoomNumber}", modifyRoomDto.RoomNumber);

            var parameters = new Dictionary<string, object>
            {
                { "p_room_id", modifyRoomDto.RoomId },
                { "p_room_number", modifyRoomDto.RoomNumber },
                { "p_category_id", modifyRoomDto.CategoryId },
                { "p_floor_id", modifyRoomDto.FloorId },
                { "p_description", modifyRoomDto.Description },
                { "p_room_img_url", modifyRoomDto.RoomImgUrl },
                { "p_status", modifyRoomDto.Status },
                { "p_updated_by", modifyRoomDto.UpdatedBy }
            };

            var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "hotel.ModifyRoom",
                parameters,
                _logger
            );

            if (StoredProcedureResult.IsSuccess)
            {
                return OperationResult<ModifyRoomDto>.Success(StoredProcedureResult.Message, modifyRoomDto);
            }
            else
            {
                return OperationResult<ModifyRoomDto>.Failure(StoredProcedureResult.Message);
            }
        }

    }
}
