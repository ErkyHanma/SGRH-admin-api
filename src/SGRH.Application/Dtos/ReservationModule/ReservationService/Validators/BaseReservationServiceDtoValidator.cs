using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.ReservationService.Validators
{
    public abstract class BaseReservationServiceDtoValidator<TDto> where TDto : BaseReservationServiceDto
    {
        public virtual OperationResult<TDto> Validate(TDto dto)
        {
            if (dto == null)
                return OperationResult<TDto>.Failure("Dto cannot be null.");

            if (dto.ReservationId <= 0)
                return OperationResult<TDto>.Failure("ReservationId must be greater than zero.");

            if (dto.ServiceId <= 0)
                return OperationResult<TDto>.Failure("ServiceId must be greater than zero.");

            return OperationResult<TDto>.Success("Base validation passed.", dto);
        }
    }
}



