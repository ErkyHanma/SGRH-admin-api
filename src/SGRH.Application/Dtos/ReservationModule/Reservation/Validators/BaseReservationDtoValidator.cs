using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public abstract class BaseReservationDtoValidator<TDto> where TDto : BaseReservationDto
    {
        public virtual OperationResult<TDto> Validate(TDto dto)
        {
            if (dto == null)
                return OperationResult<TDto>.Failure("Dto cannot be null.");

            if (dto.ClientId <= 0)
                return OperationResult<TDto>.Failure("ClientId must be greater than zero.");

            if (dto.RoomId <= 0)
                return OperationResult<TDto>.Failure("RoomId must be greater than zero.");

            if (dto.StartDate >= dto.EndDate)
                return OperationResult<TDto>.Failure("StartDate must be before EndDate.");

            if (string.IsNullOrWhiteSpace(dto.Status))
                return OperationResult<TDto>.Failure("Status is required.");

            if (dto.GuestCount <= 0)
                return OperationResult<TDto>.Failure("GuestCount must be greater than zero.");

            if (dto.PaymentAmount < 0)
                return OperationResult<TDto>.Failure("PaymentAmount cannot be negative.");

            return OperationResult<TDto>.Success("Base validation passed", dto);
        }
    }
}
