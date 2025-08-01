using SGRH.Web.Models.Hotel.RoomCategory;
using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.RoomCategory.Responses
{
    public class GetAllRoomCategoriesResponse : BaseResponse
    {
        public List<RoomCategoryModel> Data { get; set; }
    }
}