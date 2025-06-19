using SGRH.Application.Dtos.Report;
using SGRH.Application.Interfaces.Repositories.Report; // Cuidado con esto?
using SGRH.Domain.Base;// Cuidado con esto?
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Persistence.Repositories.Report
{
    public class ReportRepository : IReportRepository
    {
        public Task<OperationResult<OcuppancyReportDto>> GetOcuppancyReportAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<RatesReportDto>> GetRatesReportAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<RevenueReportDto>> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ServiceRevenueReportDto>> GetServiceRevenueReportAsync(int? categoryId = null)
        {
            throw new NotImplementedException();
        }
    }
}
