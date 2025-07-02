using FluentValidation;
using SGRH.Application.Dtos.Hotel.Rate;

namespace SGRH.Application.Dtos.Hotel.Rate.Validators
{
    public class CreateRateValidator : BaseRateValidator<CreateRateDto>
    {
        public CreateRateValidator()
        {
            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }
    }
}
