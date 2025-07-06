namespace SGRH.Application.Dtos.Hotel.RoomCategory
{
    public abstract record BaseRoomCategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public string? Amenities { get; set; }
    }
}