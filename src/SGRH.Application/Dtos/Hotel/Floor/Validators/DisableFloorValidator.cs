using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.Floor.Validators
{
    public class DisableFloorValidator : AbstractValidator<DisableFloorDto>
    {
        public DisableFloorValidator()
        {
            RuleFor(x => x.FloorId).GreaterThan(0).WithMessage("FloorId must be greater than zero.");
            RuleFor(x => x.UpdatedBy).GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
        }
    }
}