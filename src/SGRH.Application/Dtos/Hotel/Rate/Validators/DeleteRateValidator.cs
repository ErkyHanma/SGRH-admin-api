using FluentValidation;
using SGRH.Application.Dtos.Hotel.Rate;

namespace SGRH.Application.Dtos.Hotel.Rate.Validators
{
    public class DeleteRateValidator : AbstractValidator<DeleteRateDto>
    {
        public DeleteRateValidator()
        {
            RuleFor(x => x.RateId)
                .GreaterThan(0).WithMessage("RateId must be greater than zero.");

            RuleFor(x => x.DeletedBy)
                .GreaterThan(0).WithMessage("DeletedBy must be greater than zero.");
        }
    }
}
