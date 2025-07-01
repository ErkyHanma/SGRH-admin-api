using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class ReservationDtoValidator : BaseReservationDtoValidator<ReservationDto>
    {
        public override OperationResult<ReservationDto> Validate(ReservationDto dto)
        {
            var result = base.Validate(dto);
            if (!result.IsSuccess)
                return result;

            if (dto.ReservationId <= 0)
                return OperationResult<ReservationDto>.Failure("ReservationId must be greater than zero.");

            if (dto.ReservationDate == default)
                return OperationResult<ReservationDto>.Failure("ReservationDate is required.");

            if (dto.ServicesCount < 0)
                return OperationResult<ReservationDto>.Failure("ServicesCount cannot be negative.");

            if (dto.TotalServicesCost < 0)
                return OperationResult<ReservationDto>.Failure("TotalServicesCost cannot be negative.");

            if (dto.CreatedAt == default)
                return OperationResult<ReservationDto>.Failure("CreatedAt is required.");

            if (dto.UpdatedAt.HasValue && dto.UpdatedAt < dto.CreatedAt)
                return OperationResult<ReservationDto>.Failure("UpdatedAt cannot be before CreatedAt.");

            return OperationResult<ReservationDto>.Success("All fields validated! ", dto);
        }
    }
}
