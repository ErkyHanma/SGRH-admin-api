using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Dtos.ReservationModule.ReservationService.Validators;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Domain.Base;

namespace SGRH.Application.Services.ReservationModule
{
    public class ReservationServiceService : IReservationServiceService
    {
        private readonly IReservationServiceRepository _reservationServiceRepository;
        private readonly IAppLogger<ReservationServiceService> _logger;

        public ReservationServiceService(IReservationServiceRepository reservationServiceRepository, IAppLogger<ReservationServiceService> logger)
        {
            _reservationServiceRepository = reservationServiceRepository;
            _logger = logger;
        }

        public async Task<OperationResult<CreateReservationServiceDto>> AddReservationServiceAsync(CreateReservationServiceDto createReservationServiceDto)
        {
            try
            {
                _logger.Info("Creating Reservation service", createReservationServiceDto);

                var validationResult = CreateReservationServiceDtoValidator.Validate(createReservationServiceDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var creationResult = await _reservationServiceRepository.AddAsync(createReservationServiceDto);

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
                _logger.Info("Attempting to delete reservation service", deleteReservationServiceDto);

                var validationResult = DeleteReservationServiceDtoValidator.Validate(deleteReservationServiceDto);
                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var deletionResult = await _reservationServiceRepository.DeleteAsync(deleteReservationServiceDto);
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
