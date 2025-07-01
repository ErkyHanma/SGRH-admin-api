using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ServiceModule.Validators
{
    public class DeleteServiceDtoValidator
    {
        public static OperationResult<DeleteServiceDto> Validate(DeleteServiceDto dto)
        {
            if (dto == null)
                return OperationResult<DeleteServiceDto>.Failure("Dto cannot be null.");

            if (dto.ServiceId <= 0)
                return OperationResult<DeleteServiceDto>.Failure("ServiceID must be greater than zero.");

            return OperationResult<DeleteServiceDto>.Success("All fields validated! ", dto);
        }
    }
}
