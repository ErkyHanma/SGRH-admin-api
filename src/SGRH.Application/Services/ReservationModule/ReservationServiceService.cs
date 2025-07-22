using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Dtos.ReservationModule.ReservationService.Validators;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Application.UseCases.ReservationModule.ReservationService;
using SGRH.Domain.Base;

namespace SGRH.Application.Services.ReservationModule
{
    public class ReservationServiceService : IReservationServiceService
    {
        private readonly IAppLogger<ReservationServiceService> _logger;
        private readonly AddReservationServiceUseCase _addReservationServiceUseCase;
        private readonly DeleteReservationServiceUseCase _deleteReservationServiceUseCase;

        public ReservationServiceService(
            IAppLogger<ReservationServiceService> logger,
            AddReservationServiceUseCase addReservationServiceUseCase,
            DeleteReservationServiceUseCase deleteReservationServiceUseCase
            )
        {
            _logger = logger;
            _addReservationServiceUseCase = addReservationServiceUseCase;
            _deleteReservationServiceUseCase = deleteReservationServiceUseCase;
        }

        public async Task<OperationResult<CreateReservationServiceDto>> AddReservationServiceAsync(CreateReservationServiceDto createReservationServiceDto)
        {
            try
            {

                var createReservationServiceDtoValidator = new CreateReservationServiceDtoValidator();
                var validationResult = createReservationServiceDtoValidator.Validate(createReservationServiceDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var creationResult = await _addReservationServiceUseCase.AddReservationServiceAsync(createReservationServiceDto);

                if (!creationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while creating a Reservation service: {creationResult.Message}.");
                    return OperationResult<CreateReservationServiceDto>.Failure($"Error trying to create a Reservation service: {creationResult.Message}");
                }

                if (creationResult.Data is null)
                {
                    return OperationResult<CreateReservationServiceDto>.Failure("No Service found.");
                }

                return OperationResult<CreateReservationServiceDto>.Success(creationResult.Message, creationResult.Data);

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error");
                return OperationResult<CreateReservationServiceDto>.Failure($"Error creating Reservation service {ex.Message}");
            }
        }

        public async Task<OperationResult<DeleteReservationServiceDto>> DeleteReservationServiceAsync(DeleteReservationServiceDto deleteReservationServiceDto)
        {
            try
            {

                var deleteReservationServiceDtoValidator = new DeleteReservationServiceDtoValidator();
                var validationResult = deleteReservationServiceDtoValidator.Validate(deleteReservationServiceDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var deletionResult = await _deleteReservationServiceUseCase.DeleteAsync(deleteReservationServiceDto);
                if (!deletionResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"Failed to delete reservation service: {deletionResult.Message}");
                    return OperationResult<DeleteReservationServiceDto>.Failure($"An error occurred while deleting the reservation service: {deletionResult.Message}");
                }

                if (deletionResult.Data is null)
                {
                    return OperationResult<DeleteReservationServiceDto>.Failure("No reservation service found.");
                }

                return OperationResult<DeleteReservationServiceDto>.Success(deletionResult.Message, deletionResult.Data);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while deleting reservation service");
                return OperationResult<DeleteReservationServiceDto>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}
