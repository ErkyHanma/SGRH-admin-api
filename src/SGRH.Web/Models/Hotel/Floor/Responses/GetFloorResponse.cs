using SGRH.Web.Models.Hotel.Floor;
using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.Floor.Responses
{
    public class GetFloorResponse : BaseResponse
    {
        public FloorModel Data { get; set; }
    }
}