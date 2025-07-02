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

        protected override int GetFloorNumber(ModifyFloorDto dto) => dto.FloorNumber;
        protected override string GetStatus(ModifyFloorDto dto) => dto.Status;
        protected override string? GetDescription(ModifyFloorDto dto) => dto.Description;
    }
}