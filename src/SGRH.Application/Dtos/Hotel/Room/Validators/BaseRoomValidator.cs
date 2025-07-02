using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    public class BaseRoomValidator<T> : AbstractValidator<T> where T : BaseRoomDto
    {
        public BaseRoomValidator() // Inicializamos y asignamos reglas segun el tipo de dato
        {
            RuleFor(x => x.RoomNumber)
                .NotEmpty().WithMessage("Room number is required.")
                .MaximumLength(10).WithMessage("Room number can't exceed 10 characters.");

            RuleFor(x => x.Status)
               .NotEmpty().WithMessage("Status is required.")
               .Must(IsValidStatus).WithMessage("Status must be: available, maintenance, or occupied.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

            RuleFor(x => x.FloorId)
                .GreaterThan(0).WithMessage("FloorId must be greater than zero.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description can't exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.RoomImgUrl)
                .MaximumLength(500).WithMessage("Image URL can't exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.RoomImgUrl));
        }
        private static bool IsValidStatus(string status) // Validar estado de habitacion
        {
            return status == "available" || status == "maintenance" || status == "occupied";
        }
    }
}
