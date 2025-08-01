namespace SGRH.Web.Models.Hotel.RoomCategory
{
    public class EditRoomCategoryModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxCapacity { get; set; }
        public string Amenities { get; set; }
        public int CategoryId { get; set; }
        public int UpdatedBy { get; set; }
    }
}