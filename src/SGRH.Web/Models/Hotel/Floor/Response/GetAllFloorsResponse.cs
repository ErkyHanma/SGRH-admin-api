using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.Floor.Response
{
    public class GetAllFloorsResponse: BaseResponse
    {
        public List<FloorModel> data { get; set; }
    }
    
}
