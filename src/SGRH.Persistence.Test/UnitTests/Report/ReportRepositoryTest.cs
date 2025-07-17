using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Persistence.Test.Repositories.Report;

/** Report tambien hace uso de un mock manual
    Fue hecho antes de implementar FakeItEasy, es una forma de hacerlo sin usar frameworks **/

namespace SGRH.Persistence.Test.UnitTests.Report
{
    public class ReportRepositoryTest
    {
        private readonly ReportRepositoryMock _reportRepositoryMock;

        public ReportRepositoryTest() 
        {
            _reportRepositoryMock = new ReportRepositoryMock();
        }

        // GetOccupancyReportAsync
        public class GetOccupancyReportAsync : ReportRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Data()
            {
                var request = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 12, 31)
                };

                var result = await _reportRepositoryMock.GetOcuppancyReportAsync(request);

                Assert.True(result.IsSuccess);
                Assert.NotEmpty(result.Data);
            }

            [Fact]
            public async Task Should_Return_Success_Message()
            {
                var request = new ReportDateRangeRequestDto 
                {
                    StartDate = new DateTime(2025, 07, 06),
                    EndDate = new DateTime(2025, 07, 16)
                };

                var result = await _reportRepositoryMock.GetOcuppancyReportAsync(request);

                Assert.Equal("Occupancy Report Generated", result.Message);
            }
        }     

        // GetRatesReportAsync
        public class GetRatesReportAsync : ReportRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Sucess_Message()
            {
                // Arrange

                var request = new ReportDateRangeRequestDto 
                {
                    StartDate = new DateTime(2024, 04, 05),
                    EndDate = new DateTime(2024, 04, 20)
                };

                // Act

                var result = await _reportRepositoryMock.GetRatesReportAsync(request);

                // Assert

                Assert.Equal("Rates Report Generated", result.Message);
            }

            [Fact]
            public async Task Should_Return_Data()
            {
                // Arrange

                var request = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 12, 31)
                };

                // Act

                var result = await _reportRepositoryMock.GetRatesReportAsync(request);

                // Assert

                Assert.True(result.IsSuccess);
                Assert.Equal("Rates Report Generated", result.Message);
            }
        }

        // GetRevenueReportAsync
        public class GetRevenueReportAsync : ReportRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Sucess_Message()
            {
                // Arrange

                var request = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 12, 31)
                };

                // Act
                var result = await _reportRepositoryMock.GetRevenueReportAsync(request);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Revenue Report Generated", result.Message);
            }

            [Fact]
            public async Task Should_Return_Data()
            {
                // Arrange

                var request = new ReportDateRangeRequestDto
                {
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 12, 31)
                };

                // Act
                var result = await _reportRepositoryMock.GetRevenueReportAsync(request);

                // Assert

                Assert.True(result.IsSuccess);
                Assert.NotEmpty(result.Data);
            }
        }

        // GetServiceRevenueReportAsync
        public class GetServiceRevenueReportAsync : ReportRepositoryTest
        {
            [Fact]
            public async Task GetServiceRevenueReportAsync_Should_Return_Sucess_Message()
            {
                //Arrange

                var request = new ServiceRevenueRequestDto { CategoryId = 1 };

                // Act

                var result = await _reportRepositoryMock.GetServiceRevenueReportAsync(request);

                // Assert

                Assert.True(result.IsSuccess);
                Assert.Equal("Service Revenue Report" +
                    "" +
                    "" +
                    " Generated", result.Message);

            }

            [Fact]
            public async Task GetServiceRevenueReportAsync_Should_Return_Data()
            {
                //Arrange

                var request = new ServiceRevenueRequestDto { CategoryId = 1 };

                // Act

                var result = await _reportRepositoryMock.GetServiceRevenueReportAsync(request);

                // Assert

                Assert.True(result.IsSuccess);
                Assert.NotEmpty(result.Data);
            }
        }
    }
}