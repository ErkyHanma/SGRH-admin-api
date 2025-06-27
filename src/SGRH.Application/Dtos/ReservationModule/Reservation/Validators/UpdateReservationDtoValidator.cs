using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class UpdateReservationDtoValidator
    {
        public static OperationResult<UpdateReservationDto> Validate(UpdateReservationDto dto)
        {
            if (dto == null)
                return OperationResult<UpdateReservationDto>.Failure("Dto cannot be null.");

            if (dto.ReservationId <= 0)
                return OperationResult<UpdateReservationDto>.Failure("ReservationId must be greater than zero.");

            if (dto.ClientId <= 0)
                return OperationResult<UpdateReservationDto>.Failure("ClientId must be greater than zero.");

            if (dto.RoomId <= 0)
                return OperationResult<UpdateReservationDto>.Failure("RoomId must be greater than zero.");

            if (dto.StartDate >= dto.EndDate)
                return OperationResult<UpdateReservationDto>.Failure("StartDate must be before EndDate.");

            if (string.IsNullOrWhiteSpace(dto.Status))
                return OperationResult<UpdateReservationDto>.Failure("Status is required.");

            if (dto.GuestCount <= 0)
                return OperationResult<UpdateReservationDto>.Failure("GuestCount must be greater than zero.");

            if (dto.UpdatedBy <= 0)
                return OperationResult<UpdateReservationDto>.Failure("UpdatedBy must be greater than zero.");

            return OperationResult<UpdateReservationDto>.Success("All fields validated! ", dto);
        }

    }
}
