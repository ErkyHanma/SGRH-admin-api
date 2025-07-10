
using SGRH.Application.Dtos.Hotel.Room.Validators;

namespace SGRH.Application.Dtos.Hotel.Room
{
    public record ModifyRoomDto : BaseRoomDto
    {
        public int RoomId { get; set; }
        public int UpdatedBy { get; set; }
    }
}