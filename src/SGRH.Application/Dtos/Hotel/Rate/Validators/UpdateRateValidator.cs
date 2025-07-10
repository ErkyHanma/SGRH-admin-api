using FluentValidation;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Dtos.Hotel.Rate.Validators;

public class UpdateRateValidator : BaseRateValidator<UpdateRateDto>
{
    public UpdateRateValidator()
    {
        RuleFor(x => x.RateId)
            .GreaterThan(0).WithMessage("RateId is required.");

        RuleFor(x => x.UpdatedBy)
            .GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
    }
}
