using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    // Heredan de la clase BaseRoomValidator aquellos validadores que usan los mismos campos
    public class CreateRoomValidator : BaseRoomValidator<CreateRoomDto>
    {
        public CreateRoomValidator() // Implementan necesidades especificas 
        {
            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }
    }
}
