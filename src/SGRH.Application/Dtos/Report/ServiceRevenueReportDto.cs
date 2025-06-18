

namespace SGRH.Application.Dtos.Report
{
    public record ServiceRevenueReportDto
    {
        public string ServiceName { get; set; }
        public string CategoryName { get; set; }
        public long ServiceCount  { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AvgRevenuePerService { get; set; }

    }
}
