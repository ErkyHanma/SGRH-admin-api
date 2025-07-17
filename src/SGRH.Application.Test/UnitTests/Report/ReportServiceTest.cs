using FakeItEasy;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Dtos.Report;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Application.Interfaces.Mappers.Hotel;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Application.Services.Hotel;
using SGRH.Application.Services.Report;
using SGRH.Application.UseCases.Report;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** Se usa FakeItEasy para simular dependencias externas (repositorios, validadores).
 El objetivo es probar la logica de ReportService **/

namespace SGRH.Application.Test.UnitTests.Report
{
    public class ReportServiceTest
    {
        private readonly IReportRepository _reportRepository = A.Fake<IReportRepository>();
        private readonly IAppLogger<ReportService> _logger = A.Fake<IAppLogger<ReportService>>();
        private readonly IConfiguration _configuration = A.Fake<IConfiguration>();
        private readonly IReportDateMustBeCorrect<ReportDateRangeRequestDto> _dateValidator = 
                         A.Fake<IReportDateMustBeCorrect<ReportDateRangeRequestDto>>();

        private readonly ReportService _reportService;

        public ReportServiceTest()
        {
            _dateValidator = A.Fake<IReportDateMustBeCorrect<ReportDateRangeRequestDto>>();

            _reportService = new ReportService(
            _reportRepository,
            _logger,
            _configuration,
            _dateValidator
            );
        }

        //GetOcuppancyReport
        public class GetOcuppancyReport : ReportServiceTest
        {
            [Fact]
            public async Task Should_Return_Correct_Occupancy_Report()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024 - 06 - 15),
                    EndDate = new DateTime(2024 - 06 - 30)

                };

                //Act
                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Success("Date range is valid."));

                A.CallTo(() => _reportRepository.GetOcuppancyReportAsync(dateRequest))
                    .Returns(OperationResult<IEnumerable<OcuppancyReportDto>>.Success("Occupancy report generated.", new List<OcuppancyReportDto>()));

                var result = await _reportService.GetOcuppancyReport(dateRequest);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                //Arrange
                ReportDateRangeRequestDto dateRequest = null;

                //Act
                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Request cannot be null."));

                var result = await _reportService.GetOcuppancyReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Request cannot be null.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Date_Is_Invalid()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024 - 06 - 30),
                    EndDate = new DateTime(2024 - 06 - 15)
                };

                //Act
                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Start date cannot be after end date."));

                var result = await _reportService.GetOcuppancyReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Start date cannot be after end date.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Date_Is_In_Future()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2030 - 09 - 20),
                    EndDate = new DateTime(2030 - 09 - 5)

                };

                //Act
                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Dates cannot be in the future."));

                var result = await _reportService.GetOcuppancyReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Dates cannot be in the future.", result.Message);
            }
        }

        //GetRatesReport
        public class GetRatesReport : ReportServiceTest
        {
            [Fact]
            public async Task Should_Return_Correct_Rates_Report()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024 - 06 - 01),
                    EndDate = new DateTime(2024 - 06 - 10)
                };

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Success("Date range is valid."));

                A.CallTo(() => _reportRepository.GetRatesReportAsync(dateRequest))
                    .Returns(OperationResult<IEnumerable<RatesReportDto>>.Success("Rates report generated.", new List<RatesReportDto>()));

                //Act
                var result = await _reportService.GetRatesReport(dateRequest);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                //Arrange
                ReportDateRangeRequestDto dateRequest = null;

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Request cannot be null."));

                //Act
                var result = await _reportService.GetRatesReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Request cannot be null.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Date_Is_Invalid()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2022 - 09 - 30),
                    EndDate = new DateTime(2022 - 09 - 15)
                };

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Start date cannot be after end date."));

                //Act
                var result = await _reportService.GetRatesReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Start date cannot be after end date.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Date_Is_In_Future()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2030 - 09 - 20),
                    EndDate = new DateTime(2030 - 09 - 30)
                };

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Dates cannot be in the future."));

                //Act
                var result = await _reportService.GetRatesReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Dates cannot be in the future.", result.Message);
            }
        }

        //GetRevenueReport
        public class GetRevenueReport : ReportServiceTest
        {
            [Fact]
            public async Task Should_Return_Correct_Revenue_Report()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024 - 06 - 01),
                    EndDate = new DateTime(2024 - 06 - 20)
                };

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Success("Date range is valid."));

                A.CallTo(() => _reportRepository.GetRevenueReportAsync(dateRequest))
                    .Returns(OperationResult<IEnumerable<RevenueReportDto>>.Success("Revenue report generated.", new List<RevenueReportDto>()));

                //Act
                var result = await _reportService.GetRevenueReport(dateRequest);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                //Arrange
                ReportDateRangeRequestDto dateRequest = null;

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Request cannot be null."));

                //Act
                var result = await _reportService.GetRevenueReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Request cannot be null.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Date_Is_Invalid()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2023 - 06 - 29),
                    EndDate = new DateTime(2023 - 05 - 01)
                };

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Start date cannot be after end date."));

                //Act
                var result = await _reportService.GetRevenueReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Start date cannot be after end date.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Date_Is_In_Future()
            {
                //Arrange
                var dateRequest = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2030 - 02 - 6),
                    EndDate = new DateTime(2030 - 02 - 11)
                };

                A.CallTo(() => _dateValidator.Validate(dateRequest))
                    .Returns(OperationResult<string>.Failure("Dates cannot be in the future."));

                //Act
                var result = await _reportService.GetRevenueReport(dateRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Dates cannot be in the future.", result.Message);
            }
        }

        //GetServiceRevenueReport
        public class GetServiceRevenueReport : ReportServiceTest
        {
            [Fact]
            public async Task Should_Return_Correct_Revenue_Report()
            {
                //Arrange
                var serviceRevenueRequest = new ServiceRevenueRequestDto
                {
                    CategoryId = 1,
                };


                A.CallTo(() => _reportRepository.GetServiceRevenueReportAsync(serviceRevenueRequest)) // <- error
                    .Returns(OperationResult<IEnumerable<ServiceRevenueReportDto>>.Success("Revenue report generated.", new List<ServiceRevenueReportDto>()));

                //Act
                var result = await _reportService.GetServiceRevenueReport(serviceRevenueRequest);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal("Revenue report generated.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                //Arrange
                ServiceRevenueRequestDto serviceRevenueRequest = null;


                //Act
                var result = await _reportService.GetServiceRevenueReport(serviceRevenueRequest);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Null(result.Data);
                Assert.Equal("Request is null for Service Revenue Report.", result.Message);
            }
        }

    }
}
