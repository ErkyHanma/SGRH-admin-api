using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.Floor.Response
{
    public class GetFloorResponse : BaseResponse 
    {
        public FloorModel Data { get; set; }
    }
}