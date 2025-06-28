using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    // DisableRoomValidator en este caso no requiere hacer uso de la clase base debido a que no la necesita
    public class DisableRoomValidator : AbstractValidator<DisableRoomDto>
    {
        public DisableRoomValidator() // En su lugar solo implementa lo necesario
        {
            RuleFor(x => x.RoomId).GreaterThan(0);
            RuleFor(x => x.UpdatedBy).GreaterThan(0);
        }
    }

}
