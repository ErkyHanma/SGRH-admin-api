

namespace SGRH.Application.Dtos.Report
{
    public record RatesReportDto
    {
        public string CategoryName { get; set; }
        public string SeasonName { get; set; }
        public decimal NightPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
