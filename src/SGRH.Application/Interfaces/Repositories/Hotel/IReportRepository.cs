
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Hotel
{
    public interface IReportRepository 
    {
        Task<OperationResult> GetOcuppancyReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult> GetRatesReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult> GetServiceRevenueReportAsync(int? categoryId = null);

        // To export

        Task<OperationResult> ExportToPdfAsync(string reportType, object data);
        Task<OperationResult> ExportToExcelAsync(string reportType, object data);
    }
}
