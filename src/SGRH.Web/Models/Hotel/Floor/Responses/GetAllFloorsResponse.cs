using SGRH.Web.Models.Hotel.Floor;
using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.Floor.Responses
{
    public class GetAllFloorsResponse : BaseResponse
    {
        public List<FloorModel> Data { get; set; }
    }
}