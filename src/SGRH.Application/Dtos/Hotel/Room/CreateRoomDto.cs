
namespace SGRH.Application.Dtos.Hotel.Room
{
    public record CreateRoomDto
    {
        public string RoomNumber { get; set; }
        public int CategoryId { get; set; }
        public int FloorId { get; set; }
        public string? Description { get; set; }
        public string? RoomImgUrl { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }

    }
}
