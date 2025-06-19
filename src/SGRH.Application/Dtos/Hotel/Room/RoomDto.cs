using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room
{
    public record RoomDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int CategoryId { get; set; }
        public int FloorId { get; set; }
        public string? Description { get; set; }
        public string? RoomImgUrl { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}
