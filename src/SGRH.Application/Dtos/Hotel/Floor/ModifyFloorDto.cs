namespace SGRH.Application.Dtos.Hotel.Floor
{
    public record class ModifyFloorDto
    {
        public int FloorId { get; set; }
        public int FloorNumber { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } // 'active', 'inactive', 'maintenance'
        public int UpdatedBy { get; set; }
    }
}