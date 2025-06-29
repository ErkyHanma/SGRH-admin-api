namespace SGRH.Application.Dtos.Hotel.RoomCategory
{
    public record class CreateRoomCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public string? Amenities { get; set; }
        public int CreatedBy { get; set; }
    }
}