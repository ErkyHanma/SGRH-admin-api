namespace SGRH.Application.Dtos.Hotel.Floor
{
    public record class CreateFloorDto
    {
        public int FloorNumber { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } // 'active', 'inactive', 'maintenance'
        public int CreatedBy { get; set; }
    }
}