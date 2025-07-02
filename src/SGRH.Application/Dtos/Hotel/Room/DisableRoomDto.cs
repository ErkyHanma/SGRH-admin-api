
namespace SGRH.Application.Dtos.Hotel.Room
{
    public record DisableRoomDto
    {
        public int RoomId { get; set; }
        public int UpdatedBy { get; set; } // La logica del procedimiento en bd lo transfiere a DeletedBy
    }
}
