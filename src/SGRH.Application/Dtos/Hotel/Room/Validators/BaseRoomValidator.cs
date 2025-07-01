using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    public abstract class BaseRoomValidator<T> : AbstractValidator<T> where T : class
    {
        protected abstract string GetRoomNumber(T dto);
        protected abstract string GetStatus(T dto);
        protected abstract int GetCategoryId(T dto);
        protected abstract int GetFloorId(T dto);
        protected abstract string? GetDescription(T dto);
        protected abstract string? GetRoomImgUrl(T dto);

        protected BaseRoomValidator() // Inicializamos y asignamos reglas segun el tipo de dato
        {
            RuleFor(x => GetRoomNumber(x))
                .NotEmpty().WithMessage("Room number is required.")
                .MaximumLength(10).WithMessage("Room number can't exceed 10 characters.");

            RuleFor(x => GetStatus(x))
               .NotEmpty().WithMessage("Status is required.")
               .Must(IsValidStatus).WithMessage("Status must be: available, maintenance, or occupied.");

            RuleFor(x => GetCategoryId(x))
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

            RuleFor(x => GetFloorId(x))
                .GreaterThan(0).WithMessage("FloorId must be greater than zero.");

            RuleFor(x => GetDescription(x))
                .MaximumLength(255).WithMessage("Description can't exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(GetDescription(x)));

            RuleFor(x => GetRoomImgUrl(x))
                .MaximumLength(500).WithMessage("Image URL can't exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(GetRoomImgUrl(x)));
        }
        private static bool IsValidStatus(string status) // Validar estado de habitacion
        {
            return status == "available" || status == "maintenance" || status == "occupied";
        }
    }
}
