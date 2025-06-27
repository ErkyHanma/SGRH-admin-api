using SGRH.Domain.Base;


namespace SGRH.Application.Dtos.ServiceModule.Validators
{
    public static class CreateServiceDtoValidator
    {
        public static OperationResult<CreateServiceDto> Validate(CreateServiceDto createServiceDto)
        {
            if (createServiceDto == null)
                return OperationResult<CreateServiceDto>.Failure("Service entity cannot be null.");

            if (string.IsNullOrWhiteSpace(createServiceDto.Name))
                return OperationResult<CreateServiceDto>.Failure("Service name is required.");

            if (string.IsNullOrWhiteSpace(createServiceDto.Description))
                return OperationResult<CreateServiceDto>.Failure("Service description is required.");

            if (createServiceDto.Price < 0)
                return OperationResult<CreateServiceDto>.Failure("Service price cannot be negative.");

            return OperationResult<CreateServiceDto>.Success("Service validated successfully.", createServiceDto);
        }
    }
}
