using SGRH.Web.Models.Hotel.RoomCategory;
using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.RoomCategory.Responses
{
    public class GetRoomCategoryResponse : BaseResponse
    {
        public RoomCategoryModel Data { get; set; }
    }
}