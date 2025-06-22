using FluentValidation;
using Microsoft.Extensions.Logging;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.Hotel
{
    public class RoomCategoryRepository : IRoomCategoryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<RoomCategoryRepository> _logger;
        private readonly IValidator<CreateRoomCategoryDto> _createValidator;
        private readonly IValidator<ModifyRoomCategoryDto> _modifyValidator;
        private readonly IValidator<DisableRoomCategoryDto> _disableValidator;

        public RoomCategoryRepository(string connectionString, ILogger<RoomCategoryRepository> logger,
                                     IValidator<CreateRoomCategoryDto> createValidator,
                                     IValidator<ModifyRoomCategoryDto> modifyValidator,
                                     IValidator<DisableRoomCategoryDto> disableValidator)
        {
            _connectionString = connectionString;
            _logger = logger;
            _createValidator = createValidator;
            _modifyValidator = modifyValidator;
            _disableValidator = disableValidator;
        }

        public async Task<OperationResult<CreateRoomCategoryDto>> AddAsync(CreateRoomCategoryDto createRoomCategoryDto)
        {
            var validationResult = _createValidator.Validate(createRoomCategoryDto);

            if (!validationResult.IsValid)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for CreateRoomCategoryDto: {Message}", message);
                return OperationResult<CreateRoomCategoryDto>.Failure(message);
            }

            _logger.LogInformation("Creating Room Category {Name}", createRoomCategoryDto.Name);

            var parameters = new Dictionary<string, object>
            {
                {"p_name", createRoomCategoryDto.Name },
                {"p_description", createRoomCategoryDto.Description },
                {"p_max_capacity", createRoomCategoryDto.MaxCapacity },
                {"p_amenities", createRoomCategoryDto.Amenities },
                {"p_created_by", createRoomCategoryDto.CreatedBy }
            };

            var storedProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "hotel.CreateRoomCategory",
                parameters,
                _logger
            );

            if (storedProcedureResult.IsSuccess)
            {
                return OperationResult<CreateRoomCategoryDto>.Success(storedProcedureResult.Message, createRoomCategoryDto);
            }
            else
            {
                return OperationResult<CreateRoomCategoryDto>.Failure(storedProcedureResult.Message);
            }
        }

        public async Task<OperationResult<DisableRoomCategoryDto>> DeleteAsync(DisableRoomCategoryDto disableRoomCategoryDto)
        {
            var validationResult = _disableValidator.Validate(disableRoomCategoryDto);

            if (!validationResult.IsValid)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for DisableRoomCategoryDto: {Message}", message);
                return OperationResult<DisableRoomCategoryDto>.Failure(message);
            }

            _logger.LogInformation("Disabling Room Category ID {CategoryId}", disableRoomCategoryDto.CategoryId);

            var parameters = new Dictionary<string, object>
            {
                { "p_category_id", disableRoomCategoryDto.CategoryId },
                { "p_updated_by", disableRoomCategoryDto.UpdatedBy }
            };

            var storedProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "hotel.DisableRoomCategory",
                parameters,
                _logger
            );

            if (storedProcedureResult.IsSuccess)
            {
                return OperationResult<DisableRoomCategoryDto>.Success(storedProcedureResult.Message, disableRoomCategoryDto);
            }
            else
            {
                return OperationResult<DisableRoomCategoryDto>.Failure(storedProcedureResult.Message);
            }
        }

        public async Task<OperationResult<IEnumerable<RoomCategoryDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all room categories");

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetRoomCategories()",
                    reader => new RoomCategoryDto
                    {
                        CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Description = reader.GetString(reader.GetOrdinal("description")), // Changed to non-nullable string
                        MaxCapacity = reader.GetInt32(reader.GetOrdinal("max_capacity")),
                        Amenities = reader.GetString(reader.GetOrdinal("amenities")),     // Changed to non-nullable string
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by"))
                    });

                return OperationResult<IEnumerable<RoomCategoryDto>>.Success("Categorías de habitación obtenidas correctamente", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync() for RoomCategoryRepository");
                return OperationResult<IEnumerable<RoomCategoryDto>>.Failure("Error al obtener categorías de habitación.");
            }
        }

        public async Task<OperationResult<RoomCategoryDto>> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting room category by ID {CategoryId}", id);

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetRoomCategoryById(@p_category_id)",
                    reader => new RoomCategoryDto
                    {
                        CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Description = reader.GetString(reader.GetOrdinal("description")), // Changed to non-nullable string
                        MaxCapacity = reader.GetInt32(reader.GetOrdinal("max_capacity")),
                        Amenities = reader.GetString(reader.GetOrdinal("amenities")),     // Changed to non-nullable string
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by"))
                    },
                    new Dictionary<string, object>
                    {
                        { "p_category_id", id }
                    });

                if (!data.Any())
                {
                    return OperationResult<RoomCategoryDto>.Failure("Categoría de habitación no encontrada");
                }

                return OperationResult<RoomCategoryDto>.Success("Categoría de habitación obtenida correctamente", data.First());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync() for RoomCategoryRepository");
                return OperationResult<RoomCategoryDto>.Failure("Error al obtener categoría de habitación.");
            }
        }

        public async Task<OperationResult<ModifyRoomCategoryDto>> UpdateAsync(ModifyRoomCategoryDto modifyRoomCategoryDto)
        {
            var validationResult = _modifyValidator.Validate(modifyRoomCategoryDto);

            if (!validationResult.IsValid)
            {
                var message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for ModifyRoomCategoryDto: {Message}", message);
                return OperationResult<ModifyRoomCategoryDto>.Failure(message);
            }

            _logger.LogInformation("Updating Room Category {Name}", modifyRoomCategoryDto.Name);

            var parameters = new Dictionary<string, object>
            {
                { "p_category_id", modifyRoomCategoryDto.CategoryId },
                { "p_name", modifyRoomCategoryDto.Name },
                { "p_description", modifyRoomCategoryDto.Description },
                { "p_max_capacity", modifyRoomCategoryDto.MaxCapacity },
                { "p_amenities", modifyRoomCategoryDto.Amenities },
                { "p_updated_by", modifyRoomCategoryDto.UpdatedBy }
            };

            var storedProcedureResult = await StoreProcedureEx.ExecuteAsync(
                _connectionString,
                "hotel.ModifyRoomCategory",
                parameters,
                _logger
            );

            if (storedProcedureResult.IsSuccess)
            {
                return OperationResult<ModifyRoomCategoryDto>.Success(storedProcedureResult.Message, modifyRoomCategoryDto);
            }
            else
            {
                return OperationResult<ModifyRoomCategoryDto>.Failure(storedProcedureResult.Message);
            }
        }
    }
}