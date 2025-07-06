using FluentValidation;

using Microsoft.Extensions.Configuration;

using SGRH.Application.Common.Logging; 
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.Hotel
{
    public class FloorRepository : IFloorRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger<FloorRepository> _logger; 
        private readonly IValidator<CreateFloorDto> _createValidator;
        private readonly IValidator<ModifyFloorDto> _modifyValidator;
        private readonly IValidator<DisableFloorDto> _disableValidator;

        public FloorRepository(IConfiguration configuration, 
                               IAppLogger<FloorRepository> logger, 
                               IValidator<CreateFloorDto> createValidator,
                               IValidator<ModifyFloorDto> modifyValidator,
                               IValidator<DisableFloorDto> disableValidator)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("SGRH");
            //_connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

            _logger = logger;
            _createValidator = createValidator;
            _modifyValidator = modifyValidator;
            _disableValidator = disableValidator;
        }

        public async Task<OperationResult<CreateFloorDto>> AddAsync(CreateFloorDto createFloorDto)
        {
            try 
            {
                var validationResult = _createValidator.Validate(createFloorDto);

                if (!validationResult.IsValid)
                {
                    
                    return HandleValidationFailure<CreateFloorDto>(validationResult);
                }

                _logger.Info("Creating Floor Number {FloorNumber}", createFloorDto.FloorNumber); 

                var parameters = new Dictionary<string, object>
                {
                    {"p_floor_number", createFloorDto.FloorNumber },
                    {"p_description", createFloorDto.Description },
                    {"p_status", createFloorDto.Status },
                    {"p_created_by", createFloorDto.CreatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<FloorRepository>( 
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
                _logger.ErrorEx(ex, "Exception thrown during AddAsync() for FloorRepository"); 
                return OperationResult<CreateFloorDto>.Failure("An error occurred while creating the floor.");
            }
        }

        public async Task<OperationResult<DisableFloorDto>> DeleteAsync(DisableFloorDto disableFloorDto)
        {
            try 
            {
                var validationResult = _disableValidator.Validate(disableFloorDto);

                if (!validationResult.IsValid)
                {
                    
                    return HandleValidationFailure<DisableFloorDto>(validationResult);
                }

                _logger.Info("Disabling Floor ID {FloorId}", disableFloorDto.FloorId); 

                var parameters = new Dictionary<string, object>
                {
                    { "p_floor_id", disableFloorDto.FloorId },
                    { "p_updated_by", disableFloorDto.UpdatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<FloorRepository>( 
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
                _logger.ErrorEx(ex, "Exception thrown during DeleteAsync() for FloorRepository"); 
                return OperationResult<DisableFloorDto>.Failure("An error occurred while disabling the floor.");
            }
        }

        public async Task<OperationResult<IEnumerable<FloorDto>>> GetAllAsync()
        {
            try
            {
                _logger.Info("Getting all floors"); 

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
                _logger.ErrorEx(ex, "Error in GetAllAsync() for FloorRepository"); 
                return OperationResult<IEnumerable<FloorDto>>.Failure("Error al obtener pisos.");
            }
        }

        public async Task<OperationResult<FloorDto>> GetByIdAsync(int id)
        {
            try
            {
                _logger.Info("Getting floor by ID {FloorId}", id); 

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
                _logger.ErrorEx(ex, "Error in GetByIdAsync() for FloorRepository"); 
                return OperationResult<FloorDto>.Failure("Error al obtener piso.");
            }
        }

        public async Task<OperationResult<ModifyFloorDto>> UpdateAsync(ModifyFloorDto modifyFloorDto)
        {
            try 
            {
                var validationResult = _modifyValidator.Validate(modifyFloorDto);

                if (!validationResult.IsValid)
                {
                    
                    return HandleValidationFailure<ModifyFloorDto>(validationResult);
                }

                _logger.Info("Updating Floor {FloorNumber}", modifyFloorDto.FloorNumber); 
                var parameters = new Dictionary<string, object>
                {
                    { "p_floor_id", modifyFloorDto.FloorId },
                    { "p_floor_number", modifyFloorDto.FloorNumber },
                    { "p_description", modifyFloorDto.Description },
                    { "p_status", modifyFloorDto.Status },
                    { "p_updated_by", modifyFloorDto.UpdatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<FloorRepository>(
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
                _logger.ErrorEx(ex, "Exception thrown during UpdateAsync() for FloorRepository"); 
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
