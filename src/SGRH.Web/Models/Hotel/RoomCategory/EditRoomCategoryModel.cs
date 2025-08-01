namespace SGRH.Web.Models.Hotel.RoomCategory
{
    public class EditRoomCategoryModel:BaseRoomCategoryModel
    {
        public string Amenities { get; set; }
        public int CategoryId { get; set; }
        public int UpdatedBy { get; set; }
    }
}