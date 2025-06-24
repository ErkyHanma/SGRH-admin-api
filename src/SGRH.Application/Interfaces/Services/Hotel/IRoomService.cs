using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.Services.Hotel
{
    public interface IRoomService
    {
        Task<OperationResult<IEnumerable<RoomDto>>> GetRooms();
        Task<OperationResult<RoomDto>> GetRoomsById(int roomId);
        Task<OperationResult<CreateRoomDto>> CreateRoom(CreateRoomDto createRoomDto);
        Task<OperationResult<ModifyRoomDto>> UpdateRoom(ModifyRoomDto modifyRoomDto);
        Task<OperationResult<DisableRoomDto>> DeleteRoom(DisableRoomDto disableRoomDto);

    }
}
