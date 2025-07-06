using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.UseCases.Hotel.Room
{
    public class RoomMustNotBeOccupied
    {
        public OperationResult<string> Validate(RoomDto room)
        {
            if (room.Status == "occupied")     
                return OperationResult<string>.Failure("The room is occupied.");
            
            return OperationResult<string>.Success("Room is available.");

        }
    }
}
