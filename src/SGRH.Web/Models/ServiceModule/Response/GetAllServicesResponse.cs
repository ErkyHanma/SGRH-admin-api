namespace SGRH.Web.Models.ServiceModule.Response
{
    public class GetAllServicesResponse : BaseResponse<ServiceModel>
    {
        public List<ServiceModel> data { get; set; }
    }
}


