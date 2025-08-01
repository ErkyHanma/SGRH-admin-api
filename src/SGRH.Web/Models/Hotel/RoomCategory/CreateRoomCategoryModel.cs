namespace SGRH.Web.Models.Hotel.RoomCategory
{
    public class CreateRoomCategoryModel: BaseRoomCategoryModel
    {
        public string Amenities { get; set;}
        public int CreatedBy { get; set;}
    }
}