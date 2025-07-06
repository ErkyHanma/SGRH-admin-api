using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.Floor.Validators
{
    public class BaseFloorValidator<T> : AbstractValidator<T> where T : BaseFloorDto
    {
        public BaseFloorValidator() 
        {
            RuleFor(x => x.FloorNumber)
                .GreaterThan(0).WithMessage("Floor number must be greater than zero.");

            RuleFor(x => x.Status)
               .NotEmpty().WithMessage("Status is required.")
               .Must(IsValidStatus).WithMessage("Status must be: active, inactive, or maintenance.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description can't exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }

        private static bool IsValidStatus(string status)
        {
            return status == "active" || status == "inactive" || status == "maintenance";
        }
    }
}