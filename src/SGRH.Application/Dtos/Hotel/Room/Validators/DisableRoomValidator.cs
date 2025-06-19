using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    public class DisableRoomValidator : AbstractValidator<DisableRoomDto>
    {
        public DisableRoomValidator()
        {
            RuleFor(x => x.RoomId).GreaterThan(0);
            RuleFor(x => x.UpdatedBy).GreaterThan(0);
        }
    }

}
