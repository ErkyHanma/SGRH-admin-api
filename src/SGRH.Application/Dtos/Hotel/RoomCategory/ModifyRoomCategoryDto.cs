namespace SGRH.Application.Dtos.Hotel.RoomCategory
{
    public record class ModifyRoomCategoryDto
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public string? Amenities { get; set; }
        public int UpdatedBy { get; set; }
    }
}