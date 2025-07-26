namespace SGRH.Web.Models.Hotel.Rates
{
    public class RateCreateModel
    {
        public int categoryId { get; set; }
        public int seasonId { get; set; }
        public decimal nightPrice { get; set; }
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
    }

}
