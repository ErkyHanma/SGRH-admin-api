using SGRH.Application.Dtos.Report;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Report
{
    public interface IReportRepository
    {
        Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReportAsync(ReportDateRangeRequestDto request);
        Task<OperationResult<IEnumerable<RevenueReportDto>>> GetRevenueReportAsync(ReportDateRangeRequestDto request);
        Task<OperationResult<IEnumerable<RatesReportDto>>> GetRatesReportAsync(ReportDateRangeRequestDto request);
        Task<OperationResult<IEnumerable<ServiceRevenueReportDto>>> GetServiceRevenueReportAsync(ServiceRevenueRequestDto request);

    }
}
