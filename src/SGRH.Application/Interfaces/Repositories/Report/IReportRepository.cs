using SGRH.Application.Dtos.Report;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Report
{
    public interface IReportRepository
    {
        Task<OperationResult<OcuppancyReportDto>> GetOcuppancyReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult<RevenueReportDto>> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult<RatesReportDto>> GetRatesReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult<ServiceRevenueReportDto>> GetServiceRevenueReportAsync(int? categoryId = null);

        // To export

        //Task<OperationResult> ExportToPdfAsync(string reportType, object data);
        //Task<OperationResult> ExportToExcelAsync(string reportType, object data);
    }
}
