using SGRH.Domain.Base;


namespace SGRH.Application.Dtos.ServiceModule.Validators
{
    public class CreateServiceDtoValidator : BaseServiceValidator<CreateServiceDto>
    {
        public override OperationResult<CreateServiceDto> Validate(CreateServiceDto createServiceDto)
        {
            var baseResult = base.Validate(createServiceDto);
            if (!baseResult.IsSuccess)
                return baseResult;

            return OperationResult<CreateServiceDto>.Success("Service validated successfully.", createServiceDto);
        }
    }
}
