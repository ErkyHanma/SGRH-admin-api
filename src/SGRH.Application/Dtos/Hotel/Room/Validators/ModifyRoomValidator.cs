using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    // Heredan de la clase BaseRoomValidator aquellos validadores que usan los mismos campos
    public class ModifyRoomValidator : BaseRoomValidator<ModifyRoomDto>
    {
        public ModifyRoomValidator() // Implementan necesidades especificas 
        {
            RuleFor(x => x.RoomId) 
                .GreaterThan(0).WithMessage("RoomId must be greater than zero.");
            RuleFor(x => x.UpdatedBy)
                .GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
        }
    }
}
