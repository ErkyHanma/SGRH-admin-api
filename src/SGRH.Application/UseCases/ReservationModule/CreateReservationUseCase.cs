using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;


namespace SGRH.Application.UseCases.ReservationModule
{
    public class CreateReservationUseCase
    {
        private readonly IReservationRepository _reservationRepository;


        public CreateReservationUseCase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<OperationResult<CreateReservationDto>> CreateReservation(CreateReservationDto createReservationDto)
        {
            try
            {

                if (createReservationDto.StartDate <= DateTime.Now || createReservationDto.EndDate <= DateTime.Now)
                {
                    return OperationResult<CreateReservationDto>.Failure("Reservation cannot be in the past");
                }

                if (createReservationDto.StartDate >= createReservationDto.EndDate)
                {
                    return OperationResult<CreateReservationDto>.Failure("Start date must be before end date.");
                }

                var reservationIsPossible = await _reservationRepository.CheckAvailability(createReservationDto.RoomId, createReservationDto.StartDate, createReservationDto.EndDate);

                if (!reservationIsPossible.IsSuccess)
                {
                    return OperationResult<CreateReservationDto>.Failure("The room is already reserved for the selected dates.");
                }

                var creationResult = await _reservationRepository.AddAsync(createReservationDto);

                if (!creationResult.IsSuccess)
                {
                    return OperationResult<CreateReservationDto>.Failure($"Error trying to create a Reservationr {creationResult.Message ?? "Unknown Error"}");
                }

                if (creationResult.Data is null)
                {
                    return OperationResult<CreateReservationDto>.Failure("No Reservation found.");
                }

                return OperationResult<CreateReservationDto>.Success(creationResult.Message, creationResult.Data);


            }
            catch (Exception ex)
            {
                return OperationResult<CreateReservationDto>.Failure($"Something went wrong while trying to create reservation: {ex}");
            }
        }
    }
}
