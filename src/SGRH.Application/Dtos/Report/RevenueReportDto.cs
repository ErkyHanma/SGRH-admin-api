
namespace SGRH.Application.Dtos.Report
{
    public record RevenueReportDto
    {
        public DateTime ReportDate { get; set; }
        public decimal RoomRevenue { get; set; }    
        public decimal ServiceRevenue { get; set; } 
        public decimal TotalRevenue { get; set; }   

    }
}
