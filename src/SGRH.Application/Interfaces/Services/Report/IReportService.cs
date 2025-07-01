using SGRH.Application.Dtos.Report;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.Services.Report
{
    public interface IReportService
    {
        Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReport(ReportDateRangeRequestDto request);
        Task<OperationResult<IEnumerable<RevenueReportDto>>> GetRevenueReport(ReportDateRangeRequestDto request);
        Task<OperationResult<IEnumerable<RatesReportDto>>> GetRatesReport(ReportDateRangeRequestDto request);
        Task<OperationResult<IEnumerable<ServiceRevenueReportDto>>> GetServiceRevenueReport(ServiceRevenueRequestDto request);
       
    }
}
