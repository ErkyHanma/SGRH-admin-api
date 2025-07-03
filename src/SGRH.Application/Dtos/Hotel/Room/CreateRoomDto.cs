

namespace SGRH.Application.Dtos.Hotel.Room
{
    public record CreateRoomDto : BaseRoomDto
    {
        public int CreatedBy { get; set; }
    }
}
