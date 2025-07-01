using FluentValidation;
using System;

namespace SGRH.Application.Dtos.Hotel.Floor.Validators
{
    public abstract class BaseFloorValidator<T> : AbstractValidator<T> where T : class
    {
        protected abstract int GetFloorNumber(T dto);
        protected abstract string GetStatus(T dto);
        protected abstract string? GetDescription(T dto);

        protected BaseFloorValidator()
        {
            RuleFor(x => GetFloorNumber(x))
                .GreaterThan(0).WithMessage("Floor number must be greater than zero.");

            RuleFor(x => GetStatus(x))
               .NotEmpty().WithMessage("Status is required.")
               .Must(IsValidStatus).WithMessage("Status must be: active, inactive, or maintenance.");

            RuleFor(x => GetDescription(x))
                .MaximumLength(255).WithMessage("Description can't exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(GetDescription(x)));
        }

        private static bool IsValidStatus(string status)
        {
            return status == "active" || status == "inactive" || status == "maintenance";
        }
    }
}