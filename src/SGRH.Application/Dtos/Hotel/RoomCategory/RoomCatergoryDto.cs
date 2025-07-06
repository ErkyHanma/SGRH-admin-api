using System;

namespace SGRH.Application.Dtos.Hotel.RoomCategory
{
    public record RoomCategoryDto : BaseRoomCategoryDto
    {
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}