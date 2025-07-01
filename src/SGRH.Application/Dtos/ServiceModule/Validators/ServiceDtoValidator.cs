using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ServiceModule.Validators
{
    public class ServiceDtoValidator
    {
        public static OperationResult<ServiceDto> Validate(ServiceDto dto)
        {

            if (dto == null)
                return OperationResult<ServiceDto>.Failure("Service entity cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return OperationResult<ServiceDto>.Failure("Service name is required.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                return OperationResult<ServiceDto>.Failure("Service description is required.");

            if (dto.Price < 0)
                return OperationResult<ServiceDto>.Failure("Service price cannot be negative.");

            return OperationResult<ServiceDto>.Success("Service validated successfully.", dto);
        }
    }
}


