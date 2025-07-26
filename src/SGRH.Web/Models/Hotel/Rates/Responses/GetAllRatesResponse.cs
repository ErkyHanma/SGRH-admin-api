namespace SGRH.Web.Models.Hotel.Rates.Responses
{
    public class GetAllRatesResponse : BaseResponse
    {
        public List<RateModel> data { get; set; }
    }
}
