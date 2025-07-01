using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class ReservationDtoValidator
    {
        public static OperationResult<ReservationDto> Validate(ReservationDto dto)
        {
            if (dto == null)
                return OperationResult<ReservationDto>.Failure("Dto cannot be null.");

            if (dto.ReservationId <= 0)
                return OperationResult<ReservationDto>.Failure("ReservationId must be greater than zero.");

            if (dto.ClientId <= 0)
                return OperationResult<ReservationDto>.Failure("ClientId must be greater than zero.");

            if (dto.RoomId <= 0)
                return OperationResult<ReservationDto>.Failure("RoomId must be greater than zero.");

            if (dto.StartDate >= dto.EndDate)
                return OperationResult<ReservationDto>.Failure("StartDate must be before EndDate.");

            if (dto.ReservationDate == default)
                return OperationResult<ReservationDto>.Failure("ReservationDate is required.");

            if (string.IsNullOrWhiteSpace(dto.Status))
                return OperationResult<ReservationDto>.Failure("Status is required.");

            if (dto.GuestCount <= 0)
                return OperationResult<ReservationDto>.Failure("GuestCount must be greater than zero.");

            if (dto.PaymentAmount < 0)
                return OperationResult<ReservationDto>.Failure("PaymentAmount cannot be negative.");

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
