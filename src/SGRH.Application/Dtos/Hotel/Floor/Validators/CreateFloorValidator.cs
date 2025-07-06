using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.Floor.Validators
{
    public class CreateFloorValidator : BaseFloorValidator<CreateFloorDto>
    {
        public CreateFloorValidator()
        {
            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }
    }
}