namespace SGRH.Application.Dtos.Hotel.Floor
{
    public record FloorDto
    {
        public int FloorId { get; set; }
        public int FloorNumber { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } // 'active', 'inactive', 'maintenance'
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}