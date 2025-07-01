using FluentValidation;
using System;

namespace SGRH.Application.Dtos.Hotel.RoomCategory.Validators
{
    public abstract class BaseRoomCategoryValidator<T> : AbstractValidator<T> where T : class
    {
        protected abstract string GetName(T dto);
        protected abstract string GetDescription(T dto);
        protected abstract int GetMaxCapacity(T dto);
        protected abstract string GetAmenities(T dto);

        protected BaseRoomCategoryValidator()
        {
            RuleFor(x => GetName(x))
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(50).WithMessage("Category name can't exceed 50 characters.");

            RuleFor(x => GetDescription(x))
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description can't exceed 1000 characters."); // Assuming a reasonable max length for description

            RuleFor(x => GetMaxCapacity(x))
                .GreaterThan(0).WithMessage("Max capacity must be greater than zero.");

            RuleFor(x => GetAmenities(x))
                .NotEmpty().WithMessage("Amenities are required.")
                .MaximumLength(500).WithMessage("Amenities can't exceed 500 characters."); // Assuming a reasonable max length for amenities
        }
    }
}