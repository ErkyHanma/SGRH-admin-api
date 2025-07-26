namespace SGRH.Web.Models.Hotel.RoomCategory
{

    public class ModifyRoomCategoryModel : BaseRoomCategoryModel
    {
        public int CategoryId { get; set; } 
        public int UpdatedBy { get; set; }
    }
}