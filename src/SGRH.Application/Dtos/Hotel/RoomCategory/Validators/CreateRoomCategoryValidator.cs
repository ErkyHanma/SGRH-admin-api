using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.RoomCategory.Validators
{
    public class CreateRoomCategoryValidator : BaseRoomCategoryValidator<CreateRoomCategoryDto>
    {
        public CreateRoomCategoryValidator()
        {
            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }
    }
}