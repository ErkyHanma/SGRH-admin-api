using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.RoomCategory.Response
{
    public class GetRoomCategoryResponse : BaseResponse
    {
        public RoomCategoryModel Data { get; set; }
    }
}