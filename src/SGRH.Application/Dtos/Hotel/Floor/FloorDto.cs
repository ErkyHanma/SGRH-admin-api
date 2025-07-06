using System;

namespace SGRH.Application.Dtos.Hotel.Floor
{
    public record FloorDto : BaseFloorDto
    {
        public int FloorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}