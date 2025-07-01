using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ServiceModule.Validators
{
    public abstract class BaseServiceValidator<TDto> where TDto : BaseServiceDto
    {
        public virtual OperationResult<TDto> Validate(TDto dto)
        {
            if (dto == null)
                return OperationResult<TDto>.Failure("Service entity cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return OperationResult<TDto>.Failure("Service name is required.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                return OperationResult<TDto>.Failure("Service description is required.");

            if (dto.Price < 0)
                return OperationResult<TDto>.Failure("Service price cannot be negative.");


            return OperationResult<TDto>.Success("Base validation passed.", dto);
        }
    }
}
