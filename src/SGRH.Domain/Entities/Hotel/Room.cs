using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.Hotel
{
    public class Room : AuditEntity
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int FloorId { get; set; }
        public string? Description { get; set; }
        public string? RoomImgUrl { get; set; }
        public string Status { get; set; } = "available";

    }
}

