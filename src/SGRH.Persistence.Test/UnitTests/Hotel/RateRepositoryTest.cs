using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.Hotel;
using SGRH.Persistence.Test.Context;
using SGRH.Persistence.Test.Repositories;
using SGRH.Persistence.Test.Repositories.Hotel;

namespace SGRH.Persistence.Test.UnitTests.Hotel
{
    public class RateRepositoryTest
    {
        private readonly IRatesRepository _ratesRepository;
        public RateRepositoryTest()  // RateContext rateContext
        {
            _ratesRepository = new RatesRepositoryMock(new Context.RateContext());
        }

        // AddAsync
        public class AddAsync : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Error_When_Entity_Is_Null()
            {
                //Arrange 
                Rate rate = null;

                //Act

                var result = await _ratesRepository.AddAsync(rate);
                var message = "Rate is null.";

                //Assert

                Assert.IsType<OperationResult<Rate>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(message, result.Message);
            }

            [Fact]
            public async Task Should_Save_And_Return_Rate()
            {
                //Arrange 
                var rate = new Rate { CategoryId = 1, SeasonId = 1, NightPrice = 150m };

                //Act

                var result = await _ratesRepository.AddAsync(rate);


                //Assert

                Assert.IsType<OperationResult<Rate>>(result); // da error
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal(150m, result.Data.NightPrice);
            }

        }

        // UpdateAsync
        public class UpdateAsync : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Error_When_Entity_Is_Null()
            {
                //Arrange 
                Rate rate = null;

                //Act

                var result = await _ratesRepository.UpdateAsync(rate);
                var message = "Rate is null.";

                //Assert

                Assert.IsType<OperationResult<Rate>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(message, result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Rate_Is_Deleted()
            {
                //Arrange
                var rate = new Rate { CategoryId = 2, SeasonId = 2, NightPrice = 1260m };
                var added = await _ratesRepository.AddAsync(rate);

                added.Data.IsDeleted = true;

                //Act

                var result = await _ratesRepository.UpdateAsync(added.Data);

                //Assert

                Assert.False(result.IsSuccess);
                Assert.Equal("Rate not found or already deleted.", result.Message);
            }

            [Fact]
            public async Task Should_Update_Rate_Successfully()
            {
                //Arrange
                var rate = new Rate { CategoryId = 1, SeasonId = 1, NightPrice = 126m };
                var added = await _ratesRepository.AddAsync(rate);

                added.Data.NightPrice = 1260m;

                //Act

                var result = await _ratesRepository.UpdateAsync(added.Data);

                //Assert

                Assert.True(result.IsSuccess);
                Assert.Equal(1260m, result.Data.NightPrice);
            }
        }

        // DeleteAsync
        public class DeleteAsync : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Error_When_Entity_Is_Null()
            {
                //Arrange 
                Rate rate = null;

                //Act

                var result = await _ratesRepository.DeleteAsync(rate);
                var message = "Rate is null.";

                //Assert

                Assert.IsType<OperationResult<Rate>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(message, result.Message);
            }

            [Fact]
            public async Task Should_Mark_Rate_As_Deleted()
            {
                //Arrange 
                var rate = new Rate { CategoryId = 3, SeasonId = 3, NightPrice = 1500m };
                var added = await _ratesRepository.AddAsync(rate);

                //Act

                var result = await _ratesRepository.DeleteAsync(rate);

                //Assert

                Assert.IsType<OperationResult<Rate>>(result);
                Assert.True(result.IsSuccess);
                Assert.True(result.Data.IsDeleted);
                Assert.False(result.Data.IsActive);
            }

            [Fact]
            public async Task Should_Return_Error_When_Rate_Not_Found()
            {
                //Arrange

                var badRate = new Rate { RateId = 999, CategoryId = 2, SeasonId = 3 };

                //Act

                var result = await _ratesRepository.DeleteAsync(badRate);

                //Assert

                Assert.False(result.IsSuccess);
                Assert.Equal("Rate not found or already deleted.", result.Message);

            }
        }

        //GetRatesByCategoryAsync
        public class GetRatesByCategoryAsync : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Rates()
            {
                //Arrange

                Rate rate1 = new Rate { CategoryId = 3, SeasonId = 2, NightPrice = 150m };
                Rate rate2 = new Rate { CategoryId = 3, SeasonId = 1, NightPrice = 1550m };

                //Act

                var result = await _ratesRepository.GetRatesByCategoryAsync(3);

                //Assert

                Assert.True(result.IsSuccess);
                Assert.NotNull(result);
            }

            [Fact]
            public async Task Should_Return_Empty_When_Category_Not_Found()
            {
                //Act

                var result = await _ratesRepository.GetRatesByCategoryAsync(9999);

                //Assert

                Assert.True(result.IsSuccess);
                Assert.Empty(result.Data);
            }
        }
    
        // GetAllAsync
        public class GetAllAsync : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Return_All_Non_Rates()
            {
                //arrange
                var rate1 = new Rate { CategoryId = 1, SeasonId = 1, NightPrice = 100m };
                var rate2 = new Rate { CategoryId = 1, SeasonId = 2, NightPrice = 1000m };

                await _ratesRepository.AddAsync(rate1);
                await _ratesRepository.AddAsync(rate2);

                //act

                var result = await _ratesRepository.GetAllAsync();

                //assert

                Assert.True(result.IsSuccess);
                Assert.All(result.Data, r => Assert.False(r.IsDeleted));

            }
        }

        // GetAllAsync(filter)
        public class GetAllAsyncWithFilter : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Filtered_Rates()
            {
                //Arrange

                var rate1 = new Rate { CategoryId = 1, SeasonId = 3, NightPrice = 1200m };
                var rate2 = new Rate { CategoryId = 2, SeasonId = 1, NightPrice = 100m };

                //Act

                var result = await _ratesRepository.GetAllAsync(r => r.CategoryId == 2);

                //Assert

                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);

            }
            [Fact]
            public async Task Should_Not_Return_Deleted_Entities()
            {
                //Arrange

                var rate1 = new Rate { CategoryId = 99, SeasonId = 9, NightPrice = 600m, IsDeleted = true };

                //Act

                var result = await _ratesRepository.GetAllAsync(r => r.CategoryId == 99);

                //Assert

                Assert.True(result.IsSuccess);
                Assert.Empty(result.Data);

            }
        }        

        // GetByIdAsync
        public class GetByIdAsync : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Returns_Rate_When_Exists()
            {
                //arrange
                var rate = new Rate { CategoryId = 1, SeasonId = 1, NightPrice = 100m };
                var added = await _ratesRepository.AddAsync(rate);

                //act

                var result = await _ratesRepository.GetByIdAsync(added.Data.RateId);

                //assert

                Assert.IsType<OperationResult<Rate>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(100m, result.Data.NightPrice);

            }
            [Fact]
            public async Task Should_Return_Error_When_Not_Exists()
            {
                //act

                var result = await _ratesRepository.GetByIdAsync(999);

                //assert

                Assert.False(result.IsSuccess);
                Assert.Equal("Rate not found.", result.Message);

            }
        }

        // ExistsAsync
        public class ExistsAsync : RateRepositoryTest
        {
            [Fact]
            public async Task Should_Return_True_When_Condition_Matches()
            {
                //arrange
                var rate = new Rate { CategoryId = 4, SeasonId = 1, NightPrice = 100m };
                var added = await _ratesRepository.AddAsync(rate);

                //act

                var result = await _ratesRepository.ExistsAsync(r => r.CategoryId == 4);
                //assert
                Assert.True(result);

            }

            [Fact]
            public async Task Should_Return_False_When_Condition_No_Match()
            {
                //act

                var result = await _ratesRepository.ExistsAsync(r => r.CategoryId == 999);

                //assert
                Assert.False(result);
            }
        }
    }
}