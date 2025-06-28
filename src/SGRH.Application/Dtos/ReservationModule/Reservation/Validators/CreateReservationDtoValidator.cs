using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class CreateReservationDtoValidator
    {
        public static OperationResult<CreateReservationDto> Validate(CreateReservationDto dto)
        {
            if (dto == null)
            {
                return OperationResult<CreateReservationDto>.Failure("Dto cannot be null");
            }

            if (dto.ClientId <= 0)
                return OperationResult<CreateReservationDto>.Failure("ClientId is invalid");

            if (dto.RoomId <= 0)
                return OperationResult<CreateReservationDto>.Failure("RoomId is invalid");

            if (dto.StartDate >= dto.EndDate)
                return OperationResult<CreateReservationDto>.Failure("Start date must be before end date");

            if (string.IsNullOrWhiteSpace(dto.Status))
                return OperationResult<CreateReservationDto>.Failure("Status is required");

            if (dto.GuestCount <= 0)
                return OperationResult<CreateReservationDto>.Failure("Guest count must be greater than 0");


            return OperationResult<CreateReservationDto>.Success("All field validated", dto);
        }
    }
}
