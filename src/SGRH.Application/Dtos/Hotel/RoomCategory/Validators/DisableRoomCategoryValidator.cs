using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.RoomCategory.Validators
{
    public class DisableRoomCategoryValidator : AbstractValidator<DisableRoomCategoryDto>
    {
        public DisableRoomCategoryValidator()
        {
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than zero.");
            RuleFor(x => x.UpdatedBy).GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
        }
    }
}