using Microsoft.Extensions.Configuration;
using SGRH.Application.Dtos.Report;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Report;
using SGRH.Application.UseCases.Report;
using SGRH.Common.Common;
using SGRH.Domain.Base;


namespace SGRH.Application.Services.Report
{
    public sealed class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAppLogger<ReportService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ReportDateMustBeCorrect _reportDateMustBeCorrect;

        public ReportService(IReportRepository reportRepository,
                             IAppLogger<ReportService> logger,
                             IConfiguration configuration,
                             ReportDateMustBeCorrect reportDateMustBeCorrect)
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _configuration = configuration;
            _reportDateMustBeCorrect = reportDateMustBeCorrect;
        }

        public async Task<OperationResult<IEnumerable<OcuppancyReportDto>>> GetOcuppancyReport(ReportDateRangeRequestDto request)
        {
            try
            {
                var validation = _reportDateMustBeCorrect.Validate(request);
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
                var validation = _reportDateMustBeCorrect.Validate(request);
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
                var validation = _reportDateMustBeCorrect.Validate(request);
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


    }
}
