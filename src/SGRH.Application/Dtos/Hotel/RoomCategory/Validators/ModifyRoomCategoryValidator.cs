using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.RoomCategory.Validators
{
    // Hereda de BaseRoomCategoryValidator<ModifyRoomCategoryDto>
    public class ModifyRoomCategoryValidator : BaseRoomCategoryValidator<ModifyRoomCategoryDto>
    {
        public ModifyRoomCategoryValidator() // Implementa necesidades específicas
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");
            RuleFor(x => x.UpdatedBy)
                .GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
        }
    }
}