using SGRH.Web.Models.Base;

namespace SGRH.Web.Models.Hotel.Floor.Response
{
    public class ModifyFloorResponse : BaseResponse
    {
        public ModifyFloorModel Data { get; set; } // Cambiado de 'data' a 'Data'
    }
}