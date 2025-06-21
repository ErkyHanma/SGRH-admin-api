using SGRH.Application.Dtos.Report;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Report
{
    public interface IReportRepository
    {
        Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult<IEnumerable<RevenueReportDto>>> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult<IEnumerable<RatesReportDto>>> GetRatesReportAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult<IEnumerable<ServiceRevenueReportDto>>> GetServiceRevenueReportAsync(int? categoryId = null);

    }
}
