namespace SGRH.Web.Models.Hotel.RoomCategory
{
    public abstract class BaseRoomCategoryModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public string? Amenities { get; set; }
    }
}