using FluentValidation;
using SGRH.Application.Dtos.Hotel.Rate;

namespace SGRH.Application.Dtos.Hotel.Rate.Validators
{
    public class CreateRateValidator : AbstractValidator<CreateRateDto>
    {
        public CreateRateValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

            RuleFor(x => x.SeasonId)
                .GreaterThan(0).WithMessage("SeasonId must be greater than zero.");

            RuleFor(x => x.NightPrice)
                .GreaterThan(0).WithMessage("NightPrice must be greater than zero.");

            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }
    }
}
