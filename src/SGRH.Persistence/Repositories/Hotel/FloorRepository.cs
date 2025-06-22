using FluentValidation;
using Microsoft.Extensions.Logging;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.Hotel
{
    public class FloorRepository : IFloorRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<FloorRepository> _logger;
        private readonly IValidator<CreateFloorDto> _createValidator;
        private readonly IValidator<ModifyFloorDto> _modifyValidator;
        private readonly IValidator<DisableFloorDto> _disableValidator;

        public FloorRepository(string connectionString, ILogger<FloorRepository> logger,
                               IValidator<CreateFloorDto> createValidator,
                               IValidator<ModifyFloorDto> modifyValidator,
                               IValidator<DisableFloorDto> disableValidator)
        {
            _connectionString = connectionString;
            _logger = logger;
            _createValidator = createValidator;
            _modifyValidator = modifyValidator;
            _disableValidator = disableValidator;
        }

        public async Task<OperationResult<CreateFloorDto>> AddAsync(CreateFloorDto createFloorDto)
        {
            var validationResult = _createValidator.Validate(createFloorDto);

            if (!validationResult.IsValid)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for CreateFloorDto: {Message}", message);
                return OperationResult<CreateFloorDto>.Failure(message);
            }

            _logger.LogInformation("Creating Floor Number {FloorNumber}", createFloorDto.FloorNumber);

            var parameters = new Dictionary<string, object>
            {
                {"p_floor_number", createFloorDto.FloorNumber },
                {"p_description", createFloorDto.Description },
                {"p_status", createFloorDto.Status },
                {"p_created_by", createFloorDto.CreatedBy }
            };

            var storedProcedureResult = await StoreProcedureEx.ExecuteAsync(
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

        public async Task<OperationResult<DisableFloorDto>> DeleteAsync(DisableFloorDto disableFloorDto)
        {
            var validationResult = _disableValidator.Validate(disableFloorDto);

            if (!validationResult.IsValid)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for DisableFloorDto: {Message}", message);
                return OperationResult<DisableFloorDto>.Failure(message);
            }

            _logger.LogInformation("Disabling Floor ID {FloorId}", disableFloorDto.FloorId);

            var parameters = new Dictionary<string, object>
            {
                { "p_floor_id", disableFloorDto.FloorId },
                { "p_updated_by", disableFloorDto.UpdatedBy }
            };

            var storedProcedureResult = await StoreProcedureEx.ExecuteAsync(
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

        public async Task<OperationResult<IEnumerable<FloorDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all floors");

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
                _logger.LogError(ex, "Error in GetAllAsync() for FloorRepository");
                return OperationResult<IEnumerable<FloorDto>>.Failure("Error al obtener pisos.");
            }
        }

        public async Task<OperationResult<FloorDto>> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting floor by ID {FloorId}", id);

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
                _logger.LogError(ex, "Error in GetByIdAsync() for FloorRepository");
                return OperationResult<FloorDto>.Failure("Error al obtener piso.");
            }
        }

        public async Task<OperationResult<ModifyFloorDto>> UpdateAsync(ModifyFloorDto modifyFloorDto)
        {
            var validationResult = _modifyValidator.Validate(modifyFloorDto);

            if (!validationResult.IsValid)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for ModifyFloorDto: {Message}", message);
                return OperationResult<ModifyFloorDto>.Failure(message);
            }

            _logger.LogInformation("Updating Floor {FloorNumber}", modifyFloorDto.FloorNumber);

            var parameters = new Dictionary<string, object>
            {
                { "p_floor_id", modifyFloorDto.FloorId },
                { "p_floor_number", modifyFloorDto.FloorNumber },
                { "p_description", modifyFloorDto.Description },
                { "p_status", modifyFloorDto.Status },
                { "p_updated_by", modifyFloorDto.UpdatedBy }
            };

            var storedProcedureResult = await StoreProcedureEx.ExecuteAsync(
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
    }
}