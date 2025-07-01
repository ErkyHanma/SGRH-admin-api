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
        private readonly IReportRepository _reportRepository;
        private readonly IAppLogger<ReportService> _logger;
        private readonly IConfiguration _configuration;

        public ReportService(IReportRepository reportRepository, IAppLogger<ReportService> logger, IConfiguration configuration)
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReport(ReportDateRangeRequestDto request)
        {
            try
            {
                var validation = ValidateDateRange(request);
                if (!validation.IsSuccess)
                {
                    return OperationResult<IEnumerable<OcuppancyReportDto>>.Failure(validation.Message);
                }

                _logger.Info("Getting an Occupancy Report from {Start} to {End}", request.StartDate, request.EndDate);

                var operationResult = await _reportRepository.GetOcuppancyReportAsync(request);

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

        public async Task<OperationResult<IEnumerable<RatesReportDto>>> GetRatesReport(ReportDateRangeRequestDto request) 
        {
            try
            {
                var validation = ValidateDateRange(request);
                if (!validation.IsSuccess)
                {
                    return OperationResult<IEnumerable<RatesReportDto>>.Failure(validation.Message);
                }

                _logger.Info("Getting a Rates Report from {Start} to {End}", request.StartDate, request.EndDate);

                var operationResult = await _reportRepository.GetRatesReportAsync(request);

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
                var validation = ValidateDateRange(request);
                if (!validation.IsSuccess)
                {
                    return OperationResult<IEnumerable<RevenueReportDto>>.Failure(validation.Message);
                }

                _logger.Info("Getting a Revenue Report from {Start} to {End}", request.StartDate, request.EndDate);

                var operationResult = await _reportRepository.GetRevenueReportAsync(request);


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
            try
            {
                if (request == null)
                {
                    return OperationResult<IEnumerable<ServiceRevenueReportDto>>.Failure("Request is null for Service Revenue Report.");
                }

                _logger.Info("Getting Service Revenue Report for CategoryId: {0}", request.CategoryId.HasValue ? request.CategoryId.Value.ToString() : "All");

                var operationResult = await _reportRepository.GetServiceRevenueReportAsync(request);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while getting a report: {operationResult.Message}.");
                    return operationResult;
                }

                return operationResult;

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error occurred while getting Service Revenue Report.");
                return OperationResult<IEnumerable<ServiceRevenueReportDto>>.Failure("An error occurred while generating the service revenue report.");
            }
        }

        // Validaciones
        private OperationResult<string> ValidateDateRange(ReportDateRangeRequestDto request)
        {
            if (request == null)
                return OperationResult<string>.Failure("Request cannot be null.");

            if (request.StartDate > request.EndDate)
                return OperationResult<string>.Failure("Start date cannot be after end date.");

            if (request.StartDate > DateTime.Now || request.EndDate > DateTime.Now)
                return OperationResult<string>.Failure("Dates cannot be in the future.");

            return OperationResult<string>.Success("Date range is valid.");
        }
    }
}
