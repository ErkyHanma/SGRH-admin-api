namespace SGRH.Web.Models.Hotel.RoomCategory
{
    public class CreateRoomCategoryModel
    {
        public string Name { get; set;}
        public string Description { get; set;}
        public int MaxCapacity { get; set;}
        public string Amenities { get; set;}
        public int CreatedBy { get; set;}
    }
}