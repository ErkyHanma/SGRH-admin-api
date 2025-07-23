namespace SGRH.Web.Models
{
    public class RateModel
    {
        public int rateId { get; set; }
        public int categoryId { get; set; }
        public int seasonId { get; set; }
        public decimal nightPrice { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public DateTime createdAt { get; set; }
        public int createdBy { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updatedAt { get; set; }
    }

    public class GetAllRatesResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public List<RateModel> data { get; set; }
    }

    public class GetRateResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public RateModel data { get; set; }
    }
}