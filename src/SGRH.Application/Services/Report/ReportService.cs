using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Dtos.Report;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Report;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Services.Report
{
    public sealed class ReportService : IReportService
    {
        private readonly IReportRepository _reportService;
        private readonly IAppLogger<ReportService> _logger;
        private readonly IConfiguration _configuration;

        public ReportService(IReportRepository reportService, IAppLogger<ReportService> logger, IConfiguration configuration)
        {
            _reportService = reportService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReport(ReportDateRangeRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return OperationResult<IEnumerable<OcuppancyReportDto>>.Failure("Request is null for Occupancy Revenue Report.");
                }

                _logger.Info("Getting an Occupancy Report from {Start} to {End}", request.StartDate, request.EndDate);

                var operationResult = await _reportService.GetOcuppancyReportAsync(request);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while getting a report: {operationResult.Message}.");
                    return operationResult;
                }

                return operationResult;

            } 
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error generating occupancy report."); // deberia estar en el archivo de configuracion?
                return OperationResult<IEnumerable<OcuppancyReportDto>>.Failure($"An error occurred. {ex.Message}");
            }


        }

        public async Task<OperationResult<IEnumerable<RatesReportDto>>> GetRatesReport(ReportDateRangeRequestDto request) // ARREGLAR
        {
            try
            {
                if (request == null)
                    return OperationResult<IEnumerable<RatesReportDto>>.Failure("Request is null for Rates Report.");

                _logger.Info("Getting a Rates Report from {Start} to {End}", request.StartDate, request.EndDate);

                var operationResult = await _reportService.GetRatesReportAsync(request);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx("An error occurred while getting Rates Report: {Message}", operationResult.Message);
                }

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error generating rates report.");
                return OperationResult<IEnumerable<RatesReportDto>>.Failure($"An error occurred. {ex.Message}");
            }
        }


        public async Task<OperationResult<IEnumerable<RevenueReportDto>>> GetRevenueReport(ReportDateRangeRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return OperationResult<IEnumerable<RevenueReportDto>>.Failure("Request is null for Revenue Report.");
                }

                _logger.Info("Getting a Revenue Report from {Start} to {End}", request.StartDate, request.EndDate);

                var operationResult = await _reportService.GetRevenueReportAsync(request);


                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while getting a report: {operationResult.Message}.");
                    return operationResult;
                }

                return operationResult;

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error generating revenue report."); // deberia estar en el archivo de configuracion?
                return OperationResult<IEnumerable<RevenueReportDto>>.Failure($"An error occurred. {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<ServiceRevenueReportDto>>> GetServiceRevenueReport(ServiceRevenueRequestDto request)
        {
            OperationResult<IEnumerable<ServiceRevenueReportDto>> operationResult = new OperationResult<IEnumerable<ServiceRevenueReportDto>>();

            try
            {
                if (request == null)
                {
                    return OperationResult<IEnumerable<ServiceRevenueReportDto>>.Failure("Request is null for Service Revenue Report.");
                }

                _logger.Info("Getting Service Revenue Report for CategoryId: {0}", request.CategoryId.HasValue ? request.CategoryId.Value.ToString() : "All");

                if (operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while getting a report: {operationResult.Message}.");
                    return operationResult;
                }

                operationResult = await _reportService.GetServiceRevenueReportAsync(request);

                //...

                return operationResult;

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error occurred while getting Service Revenue Report.");
                operationResult = OperationResult<IEnumerable<ServiceRevenueReportDto>>.Failure("An error occurred while generating the service revenue report.");
            }
            return operationResult;
        }
    }
}
