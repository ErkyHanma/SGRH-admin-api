using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Domain.Base;

namespace SGRH.Application.UseCases.ReservationModule.ReservationService
{
    public class DeleteReservationServiceUseCase
    {
        private readonly IReservationServiceRepository _reservationServiceRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IServiceRepository _serviceRepository;

        public DeleteReservationServiceUseCase(
            IReservationRepository reservationRepository,
            IReservationServiceRepository reservationServiceRepository,
            IServiceRepository serviceRepository
            )
        {
            _reservationServiceRepository = reservationServiceRepository;
            _reservationRepository = reservationRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<OperationResult<DeleteReservationServiceDto>> DeleteAsync(DeleteReservationServiceDto deleteReservationServiceDto)
        {
            try
            {
                // Check if reservation exists
                var reservationExits = await _reservationRepository.ExistsAsync(deleteReservationServiceDto.ReservationId);

                if (!reservationExits.Data)
                {
                    return OperationResult<DeleteReservationServiceDto>.Failure(reservationExits.Message);
                }

                // Check if Service exists
                var serviceExists = await _serviceRepository.GetByIdAsync(deleteReservationServiceDto.ServiceId);

                if (!serviceExists.IsSuccess)
                {
                    return OperationResult<DeleteReservationServiceDto>.Failure($"Service with ID {deleteReservationServiceDto.ServiceId} was not found");
                }

                // Check if service is added to the reservation
                var serviceIsAdded = await _reservationServiceRepository.IsServiceAdded(deleteReservationServiceDto.ReservationId, deleteReservationServiceDto.ServiceId);


                if (!serviceIsAdded.IsSuccess || !serviceIsAdded.Data)
                {
                    return OperationResult<DeleteReservationServiceDto>.Failure($"{serviceIsAdded.Message ?? "Unknown Error"}");
                }

                var result = await _reservationServiceRepository.DeleteAsync(deleteReservationServiceDto);

                if (!result.IsSuccess)
                {
                    return OperationResult<DeleteReservationServiceDto>.Failure($"Error trying to create a Reservation {result.Message ?? "Unknown Error"}");
                }

                if (result.Data is null)
                {
                    return OperationResult<DeleteReservationServiceDto>.Failure("No Reservation found");
                }

                return OperationResult<DeleteReservationServiceDto>.Success(result.Message, result.Data);


            }
            catch (Exception ex)
            {
                return OperationResult<DeleteReservationServiceDto>.Failure($"Something went wrong while trying to create reservation: {ex}");
            }
        }
    }
}
