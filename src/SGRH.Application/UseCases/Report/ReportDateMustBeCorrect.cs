using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Domain.Base;

namespace SGRH.Application.UseCases.Report
{
    public class ReportDateMustBeCorrect : IReportDateMustBeCorrect<ReportDateRangeRequestDto>
    {
        public OperationResult<string> Validate(ReportDateRangeRequestDto request)
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