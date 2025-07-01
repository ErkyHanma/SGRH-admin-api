using FluentValidation;
// using FluentValidation.Internal; // No es necesario si no se usa directamente
using Microsoft.Extensions.Configuration; // Nuevo using
// using Microsoft.Extensions.Logging; // Reemplazado por SGRH.Application.Common.Logging
using SGRH.Application.Common.Logging; // Nuevo using
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.Hotel
{
    public class FloorRepository : IFloorRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration; // Nuevo
        private readonly IAppLogger<FloorRepository> _logger; // Cambio de ILogger a IAppLogger
        private readonly IValidator<CreateFloorDto> _createValidator;
        private readonly IValidator<ModifyFloorDto> _modifyValidator;
        private readonly IValidator<DisableFloorDto> _disableValidator;

        public FloorRepository(IConfiguration configuration, // Cambio en el constructor
                               IAppLogger<FloorRepository> logger, // Cambio en el constructor
                               IValidator<CreateFloorDto> createValidator,
                               IValidator<ModifyFloorDto> modifyValidator,
                               IValidator<DisableFloorDto> disableValidator)
        {
            _configuration = configuration; // Asignación de IConfiguration
                                            //_connectionString = _configuration.GetConnectionString("SGRH"); // Obtener connection string de IConfiguration
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

            _logger = logger;
            _createValidator = createValidator;
            _modifyValidator = modifyValidator;
            _disableValidator = disableValidator;
        }

        public async Task<OperationResult<CreateFloorDto>> AddAsync(CreateFloorDto createFloorDto)
        {
            try // Añadir try-catch
            {
                var validationResult = _createValidator.Validate(createFloorDto);

                if (!validationResult.IsValid)
                {
                    // Usa el nuevo método HandleValidationFailure
                    return HandleValidationFailure<CreateFloorDto>(validationResult);
                }

                _logger.Info("Creating Floor Number {FloorNumber}", createFloorDto.FloorNumber); // Cambio de LogInformation a Info

                var parameters = new Dictionary<string, object>
                {
                    {"p_floor_number", createFloorDto.FloorNumber },
                    {"p_description", createFloorDto.Description },
                    {"p_status", createFloorDto.Status },
                    {"p_created_by", createFloorDto.CreatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<FloorRepository>( // Cambio de tipo genérico
                    _connectionString,
                    "hotel.CreateFloor",
                    parameters,
                    _logger
                );

                if (storedProcedureResult.IsSuccess)
                {
                    return OperationResult<CreateFloorDto>.Success(storedProcedureResult.Message, createFloorDto);
                }
                else
                {
                    return OperationResult<CreateFloorDto>.Failure(storedProcedureResult.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception thrown during AddAsync() for FloorRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<CreateFloorDto>.Failure("An error occurred while creating the floor.");
            }
        }

        public async Task<OperationResult<DisableFloorDto>> DeleteAsync(DisableFloorDto disableFloorDto)
        {
            try // Añadir try-catch
            {
                var validationResult = _disableValidator.Validate(disableFloorDto);

                if (!validationResult.IsValid)
                {
                    // Usa el nuevo método HandleValidationFailure
                    return HandleValidationFailure<DisableFloorDto>(validationResult);
                }

                _logger.Info("Disabling Floor ID {FloorId}", disableFloorDto.FloorId); // Cambio de LogInformation a Info

                var parameters = new Dictionary<string, object>
                {
                    { "p_floor_id", disableFloorDto.FloorId },
                    { "p_updated_by", disableFloorDto.UpdatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<FloorRepository>( // Cambio de tipo genérico
                    _connectionString,
                    "hotel.DisableFloor",
                    parameters,
                    _logger
                );

                if (storedProcedureResult.IsSuccess)
                {
                    return OperationResult<DisableFloorDto>.Success(storedProcedureResult.Message, disableFloorDto);
                }
                else
                {
                    return OperationResult<DisableFloorDto>.Failure(storedProcedureResult.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception thrown during DeleteAsync() for FloorRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<DisableFloorDto>.Failure("An error occurred while disabling the floor.");
            }
        }

        public async Task<OperationResult<IEnumerable<FloorDto>>> GetAllAsync()
        {
            try
            {
                _logger.Info("Getting all floors"); // Cambio de LogInformation a Info

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetFloors()",
                    reader => new FloorDto
                    {
                        FloorId = reader.GetInt32(reader.GetOrdinal("floor_id")),
                        FloorNumber = reader.GetInt32(reader.GetOrdinal("floor_number")),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                        Status = reader.GetString(reader.GetOrdinal("status")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by"))
                    });

                return OperationResult<IEnumerable<FloorDto>>.Success("Pisos obtenidos correctamente", data);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error in GetAllAsync() for FloorRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<IEnumerable<FloorDto>>.Failure("Error al obtener pisos.");
            }
        }

        public async Task<OperationResult<FloorDto>> GetByIdAsync(int id)
        {
            try
            {
                _logger.Info("Getting floor by ID {FloorId}", id); // Cambio de LogInformation a Info

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetFloorById(@p_floor_id)",
                    reader => new FloorDto
                    {
                        FloorId = reader.GetInt32(reader.GetOrdinal("floor_id")),
                        FloorNumber = reader.GetInt32(reader.GetOrdinal("floor_number")),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                        Status = reader.GetString(reader.GetOrdinal("status")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by"))
                    },
                    new Dictionary<string, object>
                    {
                        { "p_floor_id", id }
                    });

                if (!data.Any())
                {
                    return OperationResult<FloorDto>.Failure("Piso no encontrado");
                }

                return OperationResult<FloorDto>.Success("Piso obtenido correctamente", data.First());
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error in GetByIdAsync() for FloorRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<FloorDto>.Failure("Error al obtener piso.");
            }
        }

        public async Task<OperationResult<ModifyFloorDto>> UpdateAsync(ModifyFloorDto modifyFloorDto)
        {
            try // Añadir try-catch
            {
                var validationResult = _modifyValidator.Validate(modifyFloorDto);

                if (!validationResult.IsValid)
                {
                    // Usa el nuevo método HandleValidationFailure
                    return HandleValidationFailure<ModifyFloorDto>(validationResult);
                }

                _logger.Info("Updating Floor {FloorNumber}", modifyFloorDto.FloorNumber); // Cambio de LogInformation a Info

                var parameters = new Dictionary<string, object>
                {
                    { "p_floor_id", modifyFloorDto.FloorId },
                    { "p_floor_number", modifyFloorDto.FloorNumber },
                    { "p_description", modifyFloorDto.Description },
                    { "p_status", modifyFloorDto.Status },
                    { "p_updated_by", modifyFloorDto.UpdatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<FloorRepository>( // Cambio de tipo genérico
                    _connectionString,
                    "hotel.ModifyFloor",
                    parameters,
                    _logger
                );

                if (storedProcedureResult.IsSuccess)
                {
                    return OperationResult<ModifyFloorDto>.Success(storedProcedureResult.Message, modifyFloorDto);
                }
                else
                {
                    return OperationResult<ModifyFloorDto>.Failure(storedProcedureResult.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception thrown during UpdateAsync() for FloorRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<ModifyFloorDto>.Failure("An error occurred while updating the floor.");
            }
        }

        private OperationResult<TDto> HandleValidationFailure<TDto>(FluentValidation.Results.ValidationResult validationResult)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            var message = string.Join("; ", errors);
            _logger.ErrorNoEx("Validation failed: {Message}", message);
            return OperationResult<TDto>.Failure(message);
        }
    }
}