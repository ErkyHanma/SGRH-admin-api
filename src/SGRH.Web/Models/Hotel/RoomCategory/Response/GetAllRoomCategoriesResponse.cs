using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.RoomCategory.Response
{
    public class GetAllRoomCategoriesResponse : BaseResponse
    {
        public List<RoomCategoryModel> Data { get; set; } 
    }
}