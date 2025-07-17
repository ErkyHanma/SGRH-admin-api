using SGRH.Application.Dtos.Report;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** ReportRepositoryMock ya viene con informacion por defecto puesto que el modulo de reportes es solo lectura
 La entrada solo se limita a fechas y a ints
 De este modo, al tener datos falsos podemos usar los dto de salida
 Y simular que se estuviese obteniendo datos de la misma BD  **/

namespace SGRH.Persistence.Test.Repositories.Report
{
    public class ReportRepositoryMock : IReportRepository
    {
        public async Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReportAsync(ReportDateRangeRequestDto request)
        {
            var mockData = new List<OcuppancyReportDto>
            {
                new() { ReportDate = request.StartDate, TotalRooms = 10, OccupiedRooms = 5, OccupancyRate = 0.5m },
                new() { ReportDate = request.EndDate, TotalRooms = 10, OccupiedRooms = 9, OccupancyRate = 0.9m  }
            };

            return OperationResult<IEnumerable<OcuppancyReportDto>>.Success("Occupancy Report Generated", mockData);
        }

        public async Task<OperationResult<IEnumerable<RatesReportDto>>> GetRatesReportAsync(ReportDateRangeRequestDto request)
        {
            var mockData = new List<RatesReportDto>
            {
                new() { CategoryName = "Deluxe", SeasonName = "Summer", NightPrice = 200m, StartDate = request.StartDate, EndDate = request.EndDate }
            };

            return OperationResult<IEnumerable<RatesReportDto>>.Success("Rates Report Generated", mockData);
        }

        public async Task<OperationResult<IEnumerable<RevenueReportDto>>> GetRevenueReportAsync(ReportDateRangeRequestDto request)
        {
            var mockData = new List<RevenueReportDto>
            {
                new() { ReportDate = request.StartDate, RoomRevenue = 1000m, ServiceRevenue = 500m, TotalRevenue = 1500m }
            };

            return OperationResult<IEnumerable<RevenueReportDto>>.Success("Revenue Report Generated", mockData);

        }

        public async Task<OperationResult<IEnumerable<ServiceRevenueReportDto>>> GetServiceRevenueReportAsync(ServiceRevenueRequestDto request)
        {
            var mockData = new List<ServiceRevenueReportDto>
            {
                new() { CategoryName = "Suite", ServiceName = "Spa", ServiceCount = 3, TotalRevenue = 300m, AvgRevenuePerService = 100m }
            };

            return OperationResult<IEnumerable<ServiceRevenueReportDto>>.Success("Service Revenue Report Generated", mockData);
        }
    }
}
