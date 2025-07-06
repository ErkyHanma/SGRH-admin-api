using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.Floor.Validators
{
    public class ModifyFloorValidator : BaseFloorValidator<ModifyFloorDto>
    {
        public ModifyFloorValidator()
        {
            RuleFor(x => x.FloorId)
                .GreaterThan(0).WithMessage("FloorId must be greater than zero.");
            RuleFor(x => x.UpdatedBy)
                .GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
        }
    }
}