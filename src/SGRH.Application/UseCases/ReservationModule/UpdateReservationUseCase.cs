using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;


namespace SGRH.Application.UseCases.ReservationModule
{
    public class UpdateReservationUseCase
    {

        private readonly IReservationRepository _reservationRepository;


        public UpdateReservationUseCase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<OperationResult<UpdateReservationDto>> UpdateReservation(UpdateReservationDto updateReservationDto)
        {
            try
            {

                if (updateReservationDto.StartDate <= DateTime.Now || updateReservationDto.EndDate <= DateTime.Now)
                {
                    return OperationResult<UpdateReservationDto>.Failure("Reservation cannot be in the past");
                }


                if (updateReservationDto.StartDate >= updateReservationDto.EndDate)
                {
                    return OperationResult<UpdateReservationDto>.Failure("Start date must be before end date.");
                }


                var reservationExits = await _reservationRepository.ExistsAsync(updateReservationDto.ReservationId);

                if (!reservationExits.Data)
                {
                    return OperationResult<UpdateReservationDto>.Failure($"The reservation with ID {updateReservationDto.ReservationId} does not exists");
                }

                var reservationIsPossible = await _reservationRepository.CheckAvailability(updateReservationDto.RoomId, updateReservationDto.StartDate, updateReservationDto.EndDate);

                if (!reservationIsPossible.IsSuccess)
                {
                    return OperationResult<UpdateReservationDto>.Failure("The room is already reserved for the selected dates.");
                }

                var result = await _reservationRepository.UpdateAsync(updateReservationDto);

                if (!result.IsSuccess)
                {
                    return OperationResult<UpdateReservationDto>.Failure($"Error trying to update a Reservation {result.Message ?? "Unknown Error"}");
                }

                if (result.Data is null)
                {
                    return OperationResult<UpdateReservationDto>.Failure("No Reservation found.");
                }

                return OperationResult<UpdateReservationDto>.Success(result.Message, result.Data);


            }
            catch (Exception ex)
            {
                return OperationResult<UpdateReservationDto>.Failure($"Something went wrong while trying to create reservation: {ex}");
            }
        }

    }
}
