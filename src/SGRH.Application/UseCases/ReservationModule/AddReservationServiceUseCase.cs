using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Domain.Base;

namespace SGRH.Application.UseCases.ReservationModule
{
    public class AddReservationServiceUseCase
    {
        private readonly IReservationServiceRepository _reservationServiceRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IServiceRepository _serviceRepository;

        public AddReservationServiceUseCase(
            IReservationRepository reservationRepository,
            IReservationServiceRepository reservationServiceRepository,
            IServiceRepository serviceRepository
            )
        {
            _reservationServiceRepository = reservationServiceRepository;
            _reservationRepository = reservationRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<OperationResult<CreateReservationServiceDto>> AddReservationServiceAsync(CreateReservationServiceDto createReservationServiceDto)
        {
            try
            {
                // Check if reservation exists
                var reservationExits = await _reservationRepository.ExistsAsync(createReservationServiceDto.ReservationId);

                if (!reservationExits.Data)
                {
                    return OperationResult<CreateReservationServiceDto>.Failure($"The reservation with ID {createReservationServiceDto.ReservationId} does not exists");
                }

                // Check if Service exists
                var serviceExists = await _serviceRepository.GetByIdAsync(createReservationServiceDto.ServiceId);

                if (!serviceExists.IsSuccess)
                {
                    return OperationResult<CreateReservationServiceDto>.Failure($"Service with ID {createReservationServiceDto.ServiceId} was not found");
                }

                // Check if service is already added to the reservation
                var serviceIsAdded = await _reservationServiceRepository.IsServiceAdded(createReservationServiceDto.ReservationId, createReservationServiceDto.ServiceId);

                if (!serviceIsAdded.IsSuccess || serviceIsAdded.Data)
                {
                    return OperationResult<CreateReservationServiceDto>.Failure($"{serviceIsAdded.Message ?? "Unknown Error"}");
                }

                var result = await _reservationServiceRepository.AddAsync(createReservationServiceDto);

                if (!result.IsSuccess)
                {
                    return OperationResult<CreateReservationServiceDto>.Failure($"Error trying to create a Reservation {result.Message ?? "Unknown Error"}");
                }

                if (result.Data is null)
                {
                    return OperationResult<CreateReservationServiceDto>.Failure("No Reservation found");
                }

                return OperationResult<CreateReservationServiceDto>.Success(result.Message, result.Data);


            }
            catch (Exception ex)
            {
                return OperationResult<CreateReservationServiceDto>.Failure($"Something went wrong while trying to create reservation: {ex}");
            }
        }


    }
}

