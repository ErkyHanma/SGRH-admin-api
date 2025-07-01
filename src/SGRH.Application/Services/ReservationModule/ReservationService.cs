using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Dtos.ReservationModule.Reservation.Validators;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Domain.Base;

namespace SGRH.Application.Services.ReservationModule
{
    public class ReservationService : IReservationService
    {

        private readonly IReservationRepository _reservationRepository;
        private readonly IAppLogger<ReservationService> _logger;

        public ReservationService(IReservationRepository reservationRepository, IAppLogger<ReservationService> logger)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ReservationDto>>> GetAllReservationAsync()
        {
            try
            {
                var result = await _reservationRepository.GetAllAsync();

                if (!result.IsSuccess)
                {
                    return OperationResult<IEnumerable<ReservationDto>>.Failure(result.Message);
                }

                if (result.Data is null)
                {
                    return OperationResult<IEnumerable<ReservationDto>>.Failure("No Reservations found.");
                }

                return OperationResult<IEnumerable<ReservationDto>>.Success(result.Message, result.Data);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while getting the reservations");
                return OperationResult<IEnumerable<ReservationDto>>.Failure($"An unexpected error occurred while trying to get the reservations: {ex.Message}");
            }
        }
        public async Task<OperationResult<ReservationDto>> GetReservationByIdAsync(int id)
        {
            try
            {
                var result = await _reservationRepository.GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx("Error while getting the reservation");
                    return OperationResult<ReservationDto>.Failure(result.Message);
                }

                if (result.Data is null)
                {
                    return OperationResult<ReservationDto>.Failure($"No Reservation found with the ID: {id}.");
                }

                return OperationResult<ReservationDto>.Success(result.Message, result.Data);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, $"Unexpected error while getting the reservation with the ID: {id} ");
                return OperationResult<ReservationDto>.Failure($"An unexpected error occurred while trying to get the reservation with the ID: {id}: {ex.Message}");
            }
        }
        public async Task<OperationResult<CreateReservationDto>> AddReservationAsync(CreateReservationDto createReservationDto)
        {
            try
            {
                _logger.Info("Creating reservation", createReservationDto);

                var validationResult = CreateReservationDtoValidator.Validate(createReservationDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var creationResult = await _reservationRepository.AddAsync(createReservationDto);

                if (!creationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while creating Reservation: {creationResult.Message}.");
                    return OperationResult<CreateReservationDto>.Failure($"Error trying to create a Reservation {creationResult.Message}");
                }

                if (creationResult.Data is null)
                {
                    return OperationResult<CreateReservationDto>.Failure("No Reservation found.");
                }

                return OperationResult<CreateReservationDto>.Success(creationResult.Message, creationResult.Data);

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error");
                return OperationResult<CreateReservationDto>.Failure($"Error creating Reservation: {ex.Message}");
            }
        }
        public async Task<OperationResult<UpdateReservationDto>> UpdateReservationAsync(UpdateReservationDto updateReservationDto)
        {
            try
            {
                _logger.Info("Updating reservation", updateReservationDto);

                var validationResult = UpdateReservationDtoValidator.Validate(updateReservationDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var result = await _reservationRepository.UpdateAsync(updateReservationDto);

                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while updating Reservation: {result.Message}.");
                    return OperationResult<UpdateReservationDto>.Failure($"Error trying to update a Reservation {result.Message}");
                }

                if (result.Data is null)
                {
                    return OperationResult<UpdateReservationDto>.Failure("No Reservation found.");
                }

                return OperationResult<UpdateReservationDto>.Success(result.Message, result.Data);

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error");
                return OperationResult<UpdateReservationDto>.Failure($"Error updating Reservation {ex.Message}");
            }
        }
        public async Task<OperationResult<DisableReservationDto>> DeleteReservationAsync(DisableReservationDto disableReservationDto)
        {
            try
            {
                _logger.Info("Deleting reservation", disableReservationDto);

                var validationResult = DisableReservationDtoValidator.Validate(disableReservationDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var result = await _reservationRepository.DeleteAsync(disableReservationDto);

                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while deleting Reservation: {result.Message}.");
                    return OperationResult<DisableReservationDto>.Failure($"Error trying to delete a Reservation {result.Message}");
                }

                if (result.Data is null)
                {
                    return OperationResult<DisableReservationDto>.Failure("No Reservation found.");
                }

                return OperationResult<DisableReservationDto>.Success(result.Message, result.Data);

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error");
                return OperationResult<DisableReservationDto>.Failure($"Error deleting Reservation {ex.Message}");
            }
        }
        public async Task<OperationResult<CheckRoomAvailabilityResultDto>> CheckAvailability(int roomId, DateTime startDate, DateTime endDate)
        {
            if (roomId <= 0)
            {
                _logger.ErrorNoEx($"Invalid room ID: {roomId}");
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure("The room ID must be a positive number.");
            }

            if (startDate >= endDate)
            {
                _logger.ErrorNoEx($"Invalid date range: startDate={startDate}, endDate={endDate}");
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure("The start date must be earlier than the end date.");
            }

            try
            {
                _logger.Info($"Checking availability for room {roomId} from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

                var result = await _reservationRepository.CheckAvailability(roomId, startDate, endDate);

                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx($"Failed to check availability: {result.Message}");
                    return OperationResult<CheckRoomAvailabilityResultDto>.Failure("An error occurred while checking room availability.");
                }

                if (result.Data is null)
                {
                    return OperationResult<CheckRoomAvailabilityResultDto>.Failure("No availability information found for the given dates.");
                }

                return OperationResult<CheckRoomAvailabilityResultDto>.Success(result.Message, result.Data);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while checking room availability");
                return OperationResult<CheckRoomAvailabilityResultDto>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}
