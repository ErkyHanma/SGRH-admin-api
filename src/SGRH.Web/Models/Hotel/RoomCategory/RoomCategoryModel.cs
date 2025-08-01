namespace SGRH.Web.Models.Hotel.RoomCategory
{
    public class RoomCategoryModel: BaseRoomCategoryModel
    {
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public string Amenities { get; set; }
    }
}
