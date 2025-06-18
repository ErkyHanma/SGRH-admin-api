

namespace SGRH.Application.Dtos.Report
{
    public record OcuppancyReportDto
    {
        public DateTime ReportDate { get; set; }
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }  
        public decimal OccupancyRate { get; set; }

    }
}
