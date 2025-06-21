using Microsoft.Extensions.Logging;
using Npgsql;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Report;
using SGRH.Application.Interfaces.Repositories.Report; 
using SGRH.Domain.Base;
using SGRH.Domain.Entities.Report;
using SGRH.Domain.Entities.UserManagement;
using SGRH.Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Persistence.Repositories.Report
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _connectionString;
        private readonly AppLogger<ReportRepository> _logger;

        public ReportRepository(string connectionString, AppLogger<ReportRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM report.GetOcuppancyReport(@p_start_date, @p_end_date)",
                    reader => new OcuppancyReportDto
                    {
                        ReportDate = reader.GetDateTime(reader.GetOrdinal("report_date")),
                        TotalRooms = reader.GetInt32(reader.GetOrdinal("total_rooms")),
                        OccupiedRooms = reader.GetInt32(reader.GetOrdinal("occupied_rooms")),
                        OccupancyRate = reader.GetDecimal(reader.GetOrdinal("occupancy_rate"))
                    },
                    new Dictionary<string, object>
                    {
                       { "p_start_date", startDate },
                       { "p_end_date", endDate }
                    });

                return OperationResult<IEnumerable<OcuppancyReportDto>>.Success("Occupancy report generated.", result);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error getting the occupancy report.");
                return OperationResult<IEnumerable<OcuppancyReportDto>>.Failure("Error getting the report.");
            }
        }

        public async Task<OperationResult<IEnumerable<RatesReportDto>>> GetRatesReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM report.GetRatesReport(@p_start_date, @p_end_date)",
                    reader => new RatesReportDto
                    {
                        CategoryName = reader.GetString(reader.GetOrdinal("category_name")),
                        SeasonName = reader.GetString(reader.GetOrdinal("season_name")),
                        NightPrice = reader.GetDecimal(reader.GetOrdinal("night_price")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("end_date"))
                    },
                    new Dictionary<string, object>
                    {
                        { "p_start_date", startDate },
                        { "p_end_date", endDate }
                    });

                return OperationResult<IEnumerable<RatesReportDto>>.Success("Rate report generated.", result);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving the rate report.");
                return OperationResult<IEnumerable<RatesReportDto>>.Failure("Error retrieving the report.");
            }
        }

        public async Task<OperationResult<IEnumerable<RevenueReportDto>>> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM report.GetRevenueReport(@p_start_date, @p_end_date)",
                    reader => new RevenueReportDto
                    {
                        ReportDate = reader.GetDateTime(reader.GetOrdinal("report_date")),
                        RoomRevenue = reader.GetDecimal(reader.GetOrdinal("room_revenue")),
                        ServiceRevenue = reader.GetDecimal(reader.GetOrdinal("service_revenue")),
                        TotalRevenue = reader.GetDecimal(reader.GetOrdinal("total_revenue"))
                    },
                    new Dictionary<string, object>
                    {
                        { "p_start_date", startDate },
                        { "p_end_date", endDate }
                    });

                return OperationResult<IEnumerable<RevenueReportDto>>.Success("Revenue report generated.", result);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error getting the revenue report.");
                return OperationResult<IEnumerable<RevenueReportDto>>.Failure("Error getting the report.");
            }
        }

        public async Task<OperationResult<IEnumerable<ServiceRevenueReportDto>>> GetServiceRevenueReportAsync(int? categoryId = null)
        {
            try
            {
                var result = await FunctionReaderEx.CallFunctionAsync(
                    _connectionString,
                    "SELECT * FROM report.GetServiceRevenueReport(@p_category_id)",
                    reader => new ServiceRevenueReportDto
                    {
                        ServiceName = reader.GetString(reader.GetOrdinal("service_name")),
                        CategoryName = reader.GetString(reader.GetOrdinal("category_name")),
                        ServiceCount = reader.GetInt64(reader.GetOrdinal("service_count")),
                        TotalRevenue = reader.GetDecimal(reader.GetOrdinal("total_revenue")),
                        AvgRevenuePerService = reader.GetDecimal(reader.GetOrdinal("avg_revenue_per_service"))
                    },
                    new Dictionary<string, object>
                    {
                        { "p_category_id", categoryId.HasValue ? categoryId.Value : DBNull.Value }  // Verifica si 'categoryId' tiene un valor. De otro modo, usa DBNull.Value

                    });

                return OperationResult<IEnumerable<ServiceRevenueReportDto>>.Success("Service report generated.", result);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error getting the service report.");
                return OperationResult<IEnumerable<ServiceRevenueReportDto>>.Failure("Error getting the report.");
            }
        }
    }
}