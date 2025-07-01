using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ServiceModule.Validators
{
    public class ServiceDtoValidator : BaseServiceValidator<ServiceDto>
    {
        public override OperationResult<ServiceDto> Validate(ServiceDto serviceDto)
        {

            var baseResult = base.Validate(serviceDto);
            if (!baseResult.IsSuccess)
                return baseResult;

            if (serviceDto.ServiceId <= 0)
                return OperationResult<ServiceDto>.Failure("ServiceID must be greater than zero.");

            return OperationResult<ServiceDto>.Success("Service validated successfully.", serviceDto);
        }
    }
}


