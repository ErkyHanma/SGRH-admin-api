using FluentValidation;
using SGRH.Application.Dtos.Hotel.Rate;

public class UpdateRateValidator : AbstractValidator<UpdateRateDto>
{
    public UpdateRateValidator()
    {
        RuleFor(x => x.RateId)
            .GreaterThan(0).WithMessage("RateId is required.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

        RuleFor(x => x.SeasonId)
            .GreaterThan(0).WithMessage("SeasonId must be greater than zero.");

        RuleFor(x => x.NightPrice)
            .GreaterThan(0).WithMessage("NightPrice must be greater than zero.");

        RuleFor(x => x.UpdatedBy)
            .GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
    }
}
