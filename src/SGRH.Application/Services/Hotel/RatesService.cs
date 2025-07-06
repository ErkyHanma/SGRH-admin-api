using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Base;
using SGRH.Application.Interfaces.Mappers.Hotel;
using FluentValidation;
using SGRH.Application.UseCases.Hotel.Rate;

namespace SGRH.Application.Services.Hotel
{
    public sealed class RatesService : IRatesService
    {
        private readonly IRatesRepository _ratesRepository;
        private readonly IRateMapper _mapper;
        private readonly IAppLogger<RatesService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IValidator<CreateRateDto> _createRateValidator;
        private readonly IValidator<UpdateRateDto> _updateRateValidator;
        private readonly IValidator<DeleteRateDto> _deleteRateValidator;
        private readonly RatesMustNotBeOverlapping _ratesMustNotBeOverlapping;

        public RatesService(
            IRatesRepository ratesRepository,
            IAppLogger<RatesService> logger,
            IRateMapper mapper,
            IConfiguration configuration,
            IValidator<CreateRateDto> createRateValidator,
            IValidator<UpdateRateDto> updateRateValidator,
            IValidator<DeleteRateDto> deleteRateValidator,
            RatesMustNotBeOverlapping ratesMustNotBeOverlapping)
        {
            _ratesRepository = ratesRepository;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _createRateValidator = createRateValidator;
            _updateRateValidator = updateRateValidator;
            _deleteRateValidator = deleteRateValidator;
            _ratesMustNotBeOverlapping = ratesMustNotBeOverlapping;
        }

        public async Task<OperationResult<CreateRateDto>> CreateRatesAsync(CreateRateDto createRateDto)
        {
            try
            {
                _logger.Info("Creating Rate for Category {0}, Season {1}", createRateDto.CategoryId, createRateDto.SeasonId);

                if (createRateDto == null)
                    return OperationResult<CreateRateDto>.Failure("Input data is required.");

                var validation = _createRateValidator.Validate(createRateDto);
                if (!validation.IsValid)
                {
                    return OperationResult<CreateRateDto>.Failure(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)));
                }

                // Regla: No puede haber dos tarifas para la misma categoría y temporada

                var overlapValidation = await _ratesMustNotBeOverlapping.Validate(createRateDto.CategoryId, createRateDto.SeasonId);

                if (!overlapValidation.IsSuccess)
                    return OperationResult<CreateRateDto>.Failure(overlapValidation.Message);

                var entity = _mapper.MapFromDto(createRateDto);
                var result = await _ratesRepository.AddAsync(entity);

                if (!result.IsSuccess || result.Data == null)
                {
                    _logger.ErrorNoEx($"Failed to create rate: {result.Message}");
                    return OperationResult<CreateRateDto>.Failure($"Error creating rate: {result.Message}");
                }

                return OperationResult<CreateRateDto>.Success(result.Message, _mapper.MapToCreatedDto(result.Data));
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while creating rate.");
                return OperationResult<CreateRateDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteRatesAsync(DeleteRateDto deleteRateDto)
        {
            try
            {
                _logger.Info("Deleting Rate with ID {0}", deleteRateDto.RateId);

                if (deleteRateDto == null)
                    return OperationResult<bool>.Failure("Input data is required.");

                var validation = _deleteRateValidator.Validate(deleteRateDto);
                if (!validation.IsValid)
                {
                    return OperationResult<bool>.Failure(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)));
                }

                var existingRate = await _ratesRepository.GetByIdAsync(deleteRateDto.RateId);

                if (!existingRate.IsSuccess || existingRate.Data == null)
                {
                    return OperationResult<bool>.Failure("Rate not found.");
                }

                _mapper.ApplyDeleteDto(existingRate.Data, deleteRateDto);

                var result = await _ratesRepository.DeleteAsync(existingRate.Data);

                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx($"Error deleting rate: {result.Message}");
                    return OperationResult<bool>.Failure($"Error deleting rate: {result.Message}");
                }

                return OperationResult<bool>.Success("Rate deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error deleting rate.");
                return OperationResult<bool>.Failure($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<RateDto>>> GetRatesAsync()
        {
            try
            {
                var result = await _ratesRepository.GetAllAsync();

                if (!result.IsSuccess || result.Data == null)
                {
                    return OperationResult<IEnumerable<RateDto>>.Failure($"Error retrieving rates: {result.Message}");
                }

                var mapped = result.Data.Select(_mapper.MapToDto);
                return OperationResult<IEnumerable<RateDto>>.Success(result.Message, mapped);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving rates.");
                return OperationResult<IEnumerable<RateDto>>.Failure($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<OperationResult<RateDto>> GetRatesByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return OperationResult<RateDto>.Failure("RateId must be greater than 0.");

                var result = await _ratesRepository.GetByIdAsync(id);

                if (!result.IsSuccess || result.Data == null)
                {
                    return OperationResult<RateDto>.Failure("Rate not found.");
                }

                return OperationResult<RateDto>.Success(result.Message, _mapper.MapToDto(result.Data));
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving rate by ID.");
                return OperationResult<RateDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<OperationResult<RateDto>> UpdateRatesAsync(UpdateRateDto updateRateDto) //Abierto a una futura implementacion "reservas activas no deben verse afectadas"

        {
            try
            {
                _logger.Info("Updating Rate with ID {0}", updateRateDto.RateId);

                if (updateRateDto == null)
                    return OperationResult<RateDto>.Failure("Input data is required.");

                var validation = _updateRateValidator.Validate(updateRateDto);
                if (!validation.IsValid)
                {
                    return OperationResult<RateDto>.Failure(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)));
                }

                var existingRate = await _ratesRepository.GetByIdAsync(updateRateDto.RateId);

                if (!existingRate.IsSuccess || existingRate.Data == null)
                {
                    return OperationResult<RateDto>.Failure("Rate not found.");
                }

                // Regla: No puede haber dos tarifas para la misma categoría y temporada

                var overlapValidation = await _ratesMustNotBeOverlapping.Validate(updateRateDto.CategoryId, updateRateDto.SeasonId);

                if (!overlapValidation.IsSuccess)
                    return OperationResult<RateDto>.Failure(overlapValidation.Message);

                _mapper.ApplyUpdateDto(existingRate.Data, updateRateDto);

                var result = await _ratesRepository.UpdateAsync(existingRate.Data);

                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx($"Error updating rate: {result.Message}");
                    return OperationResult<RateDto>.Failure($"Error updating rate: {result.Message}");
                }

                return OperationResult<RateDto>.Success(result.Message, _mapper.MapToDto(result.Data));
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error updating rate.");
                return OperationResult<RateDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
