using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Interfaces.Repositories.Hotel; 
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.Hotel
{
    public class RoomRepository : IRoomRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger<RoomRepository> _logger;
        private readonly IValidator<CreateRoomDto> _createValidator;
        private readonly IValidator<ModifyRoomDto> _modifyValidator;
        private readonly IValidator<DisableRoomDto> _disableValidator;

        public RoomRepository(IConfiguration configuration, 
                              IAppLogger<RoomRepository> logger, 
                              IValidator<CreateRoomDto> createValidator, 
                              IValidator<ModifyRoomDto> modifyValidator, 
                              IValidator<DisableRoomDto> disableValidator)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SGRH"); 
            _logger = logger;
            _createValidator = createValidator;
            _modifyValidator = modifyValidator;
            _disableValidator = disableValidator;
        }

        public async Task<OperationResult<CreateRoomDto>> AddAsync(CreateRoomDto createRoomDto)
        {
            try
            {
                // Validaciones
                var validationResult = _createValidator.Validate(createRoomDto);

                // En caso de ser invalidas
                if (!validationResult.IsValid)
                    return HandleValidationFailure<CreateRoomDto>(validationResult);

                // Logear

                _logger.Info("Creating Room {RoomNumber}", createRoomDto.RoomNumber);

                // Definir parametros

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

                // Llamar StoreProcedureEx (Ahi se encuentra la logica de los Store Procedures)

                var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync<RoomRepository>(
                    _connectionString,
                    "hotel.CreateRoom",
                    parameters,
                    _logger
                );

                // Validar 

                if (StoredProcedureResult.IsSuccess)
                {
                    return OperationResult<CreateRoomDto>.Success(StoredProcedureResult.Message, createRoomDto);
                }
                else
                {
                    return OperationResult<CreateRoomDto>.Failure(StoredProcedureResult.Message);
                }
            }
            catch (Exception ex) 
            {
                _logger.ErrorEx(ex, "Exception thrown during AddAsync()");
                return OperationResult<CreateRoomDto>.Failure("An error occurred while creating the room.");
            }
        }

        public async Task<OperationResult<DisableRoomDto>> DeleteAsync(DisableRoomDto disableRoomDto)
        {
            try
            {
                var validationResult = _disableValidator.Validate(disableRoomDto);

                if (!validationResult.IsValid)
                    return HandleValidationFailure<DisableRoomDto>(validationResult);

                _logger.Info("Disabling Room ID {RoomId}", disableRoomDto.RoomId);

                var parameters = new Dictionary<string, object>
                {
                    { "p_room_id", disableRoomDto.RoomId },
                    { "p_updated_by", disableRoomDto.UpdatedBy }
                };

                var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync<RoomRepository>(
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
            catch (Exception ex) 
            {
                _logger.ErrorEx(ex, "Exception thrown during DeleteAsync()");
                return OperationResult<DisableRoomDto>.Failure("An error occurred while disabling the room.");
            }
           
        }

        public async Task<OperationResult<IEnumerable<RoomDto>>> GetAllAsync()
        {
            try
            {
                var data = await FunctionReaderEx.CallFunctionAsync( // Datos
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

                return OperationResult<IEnumerable<RoomDto>>.Success("Rooms obtained successfully.", data);

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error in GetAllAsync()");
                return OperationResult<IEnumerable<RoomDto>>.Failure("Error obtaining rooms.");
            }
        }

        public async Task<OperationResult<RoomDto>> GetByIdAsync(int id)
        {
            try
            {
                var data = await FunctionReaderEx.CallFunctionAsync( // Datos + uso de diccionario para buscar por ID
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
                    return OperationResult<RoomDto>.Failure("Room not found");
                }

                return OperationResult<RoomDto>.Success("Room obtained successfully", data.First());
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error in GetByIdAsync()");
                return OperationResult<RoomDto>.Failure("Error obtaining room.");
            }
        }

        public async Task<OperationResult<ModifyRoomDto>> UpdateAsync(ModifyRoomDto modifyRoomDto)
        {
            try
            {
                var validationResult = _modifyValidator.Validate(modifyRoomDto);

                if (!validationResult.IsValid)
                    return HandleValidationFailure<ModifyRoomDto>(validationResult);

                _logger.Info("Updating Room {RoomNumber}", modifyRoomDto.RoomNumber);

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

                var StoredProcedureResult = await StoreProcedureEx.ExecuteAsync<RoomRepository>(
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
            catch (Exception ex)
            {
                {
                    _logger.ErrorEx(ex, "Exception thrown during UpdateAsync()");
                    return OperationResult<ModifyRoomDto>.Failure("An error occurred while updating the room.");
                }

            }

        }

        // Se llama este metodo en caso de validaciones fallidas
        private OperationResult<TDto> HandleValidationFailure<TDto>(FluentValidation.Results.ValidationResult validationResult)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage); // Mensajes de error
            var message = string.Join("; ", errors); // Los apila en cadena
            _logger.ErrorNoEx("Validation failed: {Message}", message);
            return OperationResult<TDto>.Failure(message);
        }

    }

}