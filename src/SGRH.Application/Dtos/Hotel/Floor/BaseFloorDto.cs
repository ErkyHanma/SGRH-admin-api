namespace SGRH.Application.Dtos.Hotel.Floor
{
    public abstract record BaseFloorDto
    {
        public int FloorNumber { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } // 'active', 'inactive', 'maintenance'
    }
}