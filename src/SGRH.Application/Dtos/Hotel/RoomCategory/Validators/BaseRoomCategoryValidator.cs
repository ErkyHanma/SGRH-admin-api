using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.RoomCategory.Validators
{
   
    public class BaseRoomCategoryValidator<T> : AbstractValidator<T> where T : BaseRoomCategoryDto
    {
        public BaseRoomCategoryValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(50).WithMessage("Category name can't exceed 50 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description can't exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description)); 

            RuleFor(x => x.MaxCapacity)
                .GreaterThan(0).WithMessage("Max capacity must be greater than zero.");

            RuleFor(x => x.Amenities)
                .NotEmpty().WithMessage("Amenities are required.")
                .MaximumLength(500).WithMessage("Amenities can't exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Amenities));
        }
    }
}