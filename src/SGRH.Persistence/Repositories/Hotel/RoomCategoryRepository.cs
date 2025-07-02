using FluentValidation;
// using FluentValidation.Internal; // No es necesario si no se usa directamente
using Microsoft.Extensions.Configuration; // Nuevo using
// using Microsoft.Extensions.Logging; // Reemplazado por SGRH.Application.Common.Logging
using SGRH.Application.Common.Logging; // Nuevo using
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using SGRH.Persistence.Helpers;

namespace SGRH.Persistence.Repositories.Hotel
{
    public class RoomCategoryRepository : IRoomCategoryRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration; // Nuevo
        private readonly IAppLogger<RoomCategoryRepository> _logger; // Cambio de ILogger a IAppLogger
        private readonly IValidator<CreateRoomCategoryDto> _createValidator;
        private readonly IValidator<ModifyRoomCategoryDto> _modifyValidator;
        private readonly IValidator<DisableRoomCategoryDto> _disableValidator;

        public RoomCategoryRepository(IConfiguration configuration, // Cambio en el constructor
                                     IAppLogger<RoomCategoryRepository> logger, // Cambio en el constructor
                                     IValidator<CreateRoomCategoryDto> createValidator,
                                     IValidator<ModifyRoomCategoryDto> modifyValidator,
                                     IValidator<DisableRoomCategoryDto> disableValidator)
        {
            _configuration = configuration; // Asignación de IConfiguration
            //_connectionString = _configuration.GetConnectionString("SGRH"); // Obtener connection string de IConfiguration

            _connectionString = _configuration.GetConnectionString("SGRHConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");
            _logger = logger;
            _createValidator = createValidator;
            _modifyValidator = modifyValidator;
            _disableValidator = disableValidator;
        }

        public async Task<OperationResult<CreateRoomCategoryDto>> AddAsync(CreateRoomCategoryDto createRoomCategoryDto)
        {
            try // Añadir try-catch
            {
                var validationResult = _createValidator.Validate(createRoomCategoryDto);

                if (!validationResult.IsValid)
                {
                    // Usa el nuevo método HandleValidationFailure
                    return HandleValidationFailure<CreateRoomCategoryDto>(validationResult);
                }

                _logger.Info("Creating Room Category {Name}", createRoomCategoryDto.Name); // Cambio de LogInformation a Info

                var parameters = new Dictionary<string, object>
                {
                    {"p_name", createRoomCategoryDto.Name },
                    {"p_description", createRoomCategoryDto.Description },
                    {"p_max_capacity", createRoomCategoryDto.MaxCapacity },
                    {"p_amenities", createRoomCategoryDto.Amenities },
                    {"p_created_by", createRoomCategoryDto.CreatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<RoomCategoryRepository>( // Cambio de tipo genérico
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
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception thrown during AddAsync() for RoomCategoryRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<CreateRoomCategoryDto>.Failure("An error occurred while creating the room category.");
            }
        }

        public async Task<OperationResult<DisableRoomCategoryDto>> DeleteAsync(DisableRoomCategoryDto disableRoomCategoryDto)
        {
            try // Añadir try-catch
            {
                var validationResult = _disableValidator.Validate(disableRoomCategoryDto);

                if (!validationResult.IsValid)
                {
                    // Usa el nuevo método HandleValidationFailure
                    return HandleValidationFailure<DisableRoomCategoryDto>(validationResult);
                }

                _logger.Info("Disabling Room Category ID {CategoryId}", disableRoomCategoryDto.CategoryId); // Cambio de LogInformation a Info

                var parameters = new Dictionary<string, object>
                {
                    { "p_category_id", disableRoomCategoryDto.CategoryId },
                    { "p_updated_by", disableRoomCategoryDto.UpdatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<RoomCategoryRepository>( // Cambio de tipo genérico
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
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception thrown during DeleteAsync() for RoomCategoryRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<DisableRoomCategoryDto>.Failure("An error occurred while disabling the room category.");
            }
        }

        public async Task<OperationResult<IEnumerable<RoomCategoryDto>>> GetAllAsync()
        {
            try
            {
                _logger.Info("Getting all room categories"); // Cambio de LogInformation a Info

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetRoomCategories()",
                    reader => new RoomCategoryDto
                    {
                        CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        MaxCapacity = reader.GetInt32(reader.GetOrdinal("max_capacity")),
                        Amenities = reader.GetString(reader.GetOrdinal("amenities")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by"))
                    });

                return OperationResult<IEnumerable<RoomCategoryDto>>.Success("Categorías de habitación obtenidas correctamente", data);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error in GetAllAsync() for RoomCategoryRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<IEnumerable<RoomCategoryDto>>.Failure("Error al obtener categorías de habitación.");
            }
        }

        public async Task<OperationResult<RoomCategoryDto>> GetByIdAsync(int id)
        {
            try
            {
                _logger.Info("Getting room category by ID {CategoryId}", id); // Cambio de LogInformation a Info

                var data = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM hotel.GetRoomCategoryById(@p_category_id)",
                    reader => new RoomCategoryDto
                    {
                        CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        MaxCapacity = reader.GetInt32(reader.GetOrdinal("max_capacity")),
                        Amenities = reader.GetString(reader.GetOrdinal("amenities")),
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
                _logger.ErrorEx(ex, "Error in GetByIdAsync() for RoomCategoryRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<RoomCategoryDto>.Failure("Error al obtener categoría de habitación.");
            }
        }

        public async Task<OperationResult<ModifyRoomCategoryDto>> UpdateAsync(ModifyRoomCategoryDto modifyRoomCategoryDto)
        {
            try // Añadir try-catch
            {
                var validationResult = _modifyValidator.Validate(modifyRoomCategoryDto);

                if (!validationResult.IsValid)
                {
                    // Usa el nuevo método HandleValidationFailure
                    return HandleValidationFailure<ModifyRoomCategoryDto>(validationResult);
                }

                _logger.Info("Updating Room Category {Name}", modifyRoomCategoryDto.Name); // Cambio de LogInformation a Info

                var parameters = new Dictionary<string, object>
                {
                    { "p_category_id", modifyRoomCategoryDto.CategoryId },
                    { "p_name", modifyRoomCategoryDto.Name },
                    { "p_description", modifyRoomCategoryDto.Description },
                    { "p_max_capacity", modifyRoomCategoryDto.MaxCapacity },
                    { "p_amenities", modifyRoomCategoryDto.Amenities },
                    { "p_updated_by", modifyRoomCategoryDto.UpdatedBy }
                };

                var storedProcedureResult = await StoreProcedureEx.ExecuteAsync<RoomCategoryRepository>( // Cambio de tipo genérico
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
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Exception thrown during UpdateAsync() for RoomCategoryRepository"); // Cambio de LogError a ErrorEx
                return OperationResult<ModifyRoomCategoryDto>.Failure("An error occurred while updating the room category.");
            }
        }

        // Nuevo método para manejar fallos de validación, replicando el de RoomRepository
        private OperationResult<TDto> HandleValidationFailure<TDto>(FluentValidation.Results.ValidationResult validationResult)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            var message = string.Join("; ", errors);
            _logger.ErrorNoEx("Validation failed: {Message}", message);
            return OperationResult<TDto>.Failure(message);
        }
    }
}