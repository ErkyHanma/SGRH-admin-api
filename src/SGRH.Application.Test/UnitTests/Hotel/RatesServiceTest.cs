using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Interfaces.Mappers.Hotel;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Application.Services.Hotel;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.Hotel;
using SGRH.Domain.Entities.Report;
using SGRH.Domain.Entities.UserManagement;
using System;
using Xunit;

/** Se usa FakeItEasy para simular dependencias externas (repositorios, validadores).
 El objetivo es probar la logica de RatesService **/

namespace SGRH.Application.Test.UnitTests.Hotel
{
    public class RatesServiceTest
    {
        private readonly IRatesRepository _ratesRepository = A.Fake<IRatesRepository>();
        private readonly IRateMapper _rateMapper = A.Fake<IRateMapper>();
        private readonly IAppLogger<RatesService> _logger = A.Fake<IAppLogger<RatesService>>();
        private readonly IConfiguration _configuration = A.Fake<IConfiguration>();
        private readonly IValidator<CreateRateDto> _createRateValidator = A.Fake<IValidator<CreateRateDto>>();
        private readonly IValidator<UpdateRateDto> _updateRateValidator = A.Fake<IValidator<UpdateRateDto>>();
        private readonly IValidator<DeleteRateDto> _deleteRateValidator = A.Fake<IValidator<DeleteRateDto>>();
        private readonly IMustNotBeOverlapping<int> _overlapValidator = A.Fake<IMustNotBeOverlapping<int>>();
        private readonly IMustExistValidator<int> _categoryValidator = A.Fake<IMustExistValidator<int>>();

        private readonly RatesService _ratesService;

        public RatesServiceTest()
        {
            _ratesService = new RatesService(
            _ratesRepository,
            _logger,
            _rateMapper,
            _configuration,
            _createRateValidator,
            _updateRateValidator,
            _deleteRateValidator,
            _overlapValidator,
            _categoryValidator
            );
        }

        //CreateRatesAsync

        public class CreateRatesAsync : RatesServiceTest
        {
            [Fact]
            public async Task Should_Succeed_When_Valid()
            {
                //Arrange 
                var createRateDto = new CreateRateDto
                {

                    CategoryId = 1,
                    SeasonId = 2,
                    NightPrice = 2000m
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(createRateDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _createRateValidator.Validate(createRateDto))
                    .Returns(new ValidationResult());

                A.CallTo(() => _overlapValidator.Validate(createRateDto.CategoryId, createRateDto.SeasonId))
                    .Returns(OperationResult<string>.Success("No overlapping rates found."));

                A.CallTo(() => _rateMapper.MapFromDto(createRateDto))
                    .Returns(new Rate { CategoryId = createRateDto.CategoryId });

                A.CallTo(() => _ratesRepository.AddAsync(A<Rate>.Ignored))
                    .Returns(OperationResult<Rate>.Success("Rate added successfully.", new Rate()));

                A.CallTo(() => _rateMapper.MapToCreatedDto(A<Rate>.Ignored))
                    .Returns(createRateDto);

                var result = await _ratesService.CreateRatesAsync(createRateDto);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal("Rate added successfully.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Dto_Is_Null()
            {
                //Act
                var result = await _ratesService.CreateRatesAsync(null);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Null(result.Data);
            }

            [Fact]
            public async Task Should_Fail_When_Category_Does_Not_Exist()
            {
                //Arrange 
                var createRateDto = new CreateRateDto
                {

                    CategoryId = 9999
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(createRateDto.CategoryId))
                    .Returns(OperationResult<string>.Failure("CategoryId 9999 does not exist."));

                var result = await _ratesService.CreateRatesAsync(createRateDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("CategoryId 9999 does not exist.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Validation_Fails()
            {
                //Arrange
                var createRateDto = new CreateRateDto
                {

                    CategoryId = 1,
                    SeasonId = 1,
                    NightPrice = 0m

                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(createRateDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _createRateValidator.Validate(createRateDto))
                    .Returns(new ValidationResult(new List<ValidationFailure> {
                new ValidationFailure("NightPrice", "Price must be greater than zero.")
                    }));

                var result = await _ratesService.CreateRatesAsync(createRateDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("Price must be greater than zero.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Overlapping_Rate_Exists()
            {
                //Arrange
                var createRateDto = new CreateRateDto
                {

                    CategoryId = 1,
                    SeasonId = 2

                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(createRateDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _createRateValidator.Validate(createRateDto))
                    .Returns(new ValidationResult());

                A.CallTo(() => _overlapValidator.Validate(createRateDto.CategoryId, createRateDto.SeasonId))
                    .Returns(OperationResult<string>.Failure("A rate already exists for this category and season."));

                var result = await _ratesService.CreateRatesAsync(createRateDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("A rate already exists for this category and season.", result.Message);
            }
        }

        //UpdateRatesAsync
        public class UpdateRatesAsync : RatesServiceTest
        {
            [Fact]
            public async Task Should_Succeed_When_Valid()
            {
                //Arrange 
                var updateRateDto = new UpdateRateDto
                {
                    RateId = 5,
                    CategoryId = 2,
                    SeasonId = 4,
                    NightPrice = 2500m,
                    UpdatedBy = 7,
                    UpdatedAt = DateTime.Now
                };

                var existingRate = new Rate
                {
                    RateId = 5,
                    CategoryId = 3,
                    SeasonId = 3,
                    NightPrice = 500m,
                    CreatedBy = 7,
                    CreatedAt = DateTime.Now

                };


                //Act
                A.CallTo(() => _categoryValidator.Validate(updateRateDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _updateRateValidator.Validate(updateRateDto))
                    .Returns(new ValidationResult());

                A.CallTo(() => _overlapValidator.Validate(updateRateDto.CategoryId, updateRateDto.SeasonId))
                    .Returns(OperationResult<string>.Success("No overlapping rates found."));

                A.CallTo(() => _ratesRepository.GetByIdAsync(updateRateDto.RateId))
                    .Returns(OperationResult<Rate>.Success("Found", existingRate));

                A.CallTo(() => _rateMapper.ApplyUpdateDto(existingRate, updateRateDto))
                    .DoesNothing();

                A.CallTo(() => _ratesRepository.UpdateAsync(existingRate))
                    .Returns(OperationResult<Rate>.Success("Rate updated successfully.", existingRate));

                A.CallTo(() => _rateMapper.MapToDto(existingRate))
                    .Returns(new RateDto { RateId = 5, NightPrice = 2500m });

                var result = await _ratesService.UpdateRatesAsync(updateRateDto);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal(5, result.Data.RateId);
                Assert.Equal(2500m, result.Data.NightPrice);
                Assert.Equal("Rate updated successfully.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Dto_Is_Null()
            {
                //Act
                var result = await _ratesService.UpdateRatesAsync(null);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Null(result.Data);
            }

            [Fact]
            public async Task Should_Fail_When_Category_Does_Not_Exist()
            {
                //Arrange 
                var updateRateDto = new UpdateRateDto
                {
                    CategoryId = 9999,
                    RateId = 1,
                    SeasonId = 1,
                    NightPrice = 500m,
                    UpdatedBy = 5,
                    UpdatedAt = DateTime.Now,
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(updateRateDto.CategoryId))
                    .Returns(OperationResult<string>.Failure("CategoryId 9999 does not exist."));

                var result = await _ratesService.UpdateRatesAsync(updateRateDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("CategoryId 9999 does not exist.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Validation_Fails()
            {
                //Arrange
                var updateRateDto = new UpdateRateDto
                {
                    RateId = 1,
                    CategoryId = 1,
                    SeasonId = 1,
                    NightPrice = 0m,
                    UpdatedBy = 6,
                    UpdatedAt = DateTime.Now,
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(updateRateDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _updateRateValidator.Validate(updateRateDto))
                    .Returns(new ValidationResult(new List<ValidationFailure> {
                new ValidationFailure("NightPrice", "Price must be greater than zero.")
                    }));

                var result = await _ratesService.UpdateRatesAsync(updateRateDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("Price must be greater than zero.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Overlapping_Rate_Exists()
            {
                //Arrange
                var updateRateDto = new UpdateRateDto
                {
                    RateId = 6,
                    CategoryId = 2,
                    SeasonId = 4,
                    NightPrice = 250m,
                    UpdatedBy = 7,
                    UpdatedAt = DateTime.Now,
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(updateRateDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _updateRateValidator.Validate(updateRateDto))
                    .Returns(new ValidationResult());

                A.CallTo(() => _ratesRepository.GetByIdAsync(updateRateDto.RateId))
                    .Returns(OperationResult<Rate>.Success("Found", new Rate()));

                A.CallTo(() => _overlapValidator.Validate(updateRateDto.CategoryId, updateRateDto.SeasonId))
                    .Returns(OperationResult<string>.Failure("A rate already exists for this category and season."));

                var result = await _ratesService.UpdateRatesAsync(updateRateDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("A rate already exists for this category and season.", result.Message);
            }
        }

        //DeleteRatesAsync
        public class DeleteRatesAsync : RatesServiceTest
        {
            [Fact]
            public async Task Should_Succeed_When_Valid()
            {
                //Arrange 
                var deleteRatesAsync = new DeleteRateDto
                {
                    RateId = 5,
                    DeletedBy = 7

                };

                var existingRate = new Rate
                {
                    RateId = 4,
                    CategoryId = 2,
                    SeasonId = 4,
                    NightPrice = 100m,
                    CreatedBy = 7,
                    CreatedAt = DateTime.Now

                };


                //Act
                A.CallTo(() => _deleteRateValidator.Validate(deleteRatesAsync))
                    .Returns(new ValidationResult());

                A.CallTo(() => _ratesRepository.GetByIdAsync(deleteRatesAsync.RateId))
                    .Returns(OperationResult<Rate>.Success("Found", existingRate));

                A.CallTo(() => _rateMapper.ApplyDeleteDto(existingRate, deleteRatesAsync))
                    .DoesNothing();

                A.CallTo(() => _ratesRepository.DeleteAsync(existingRate))
                    .Returns(OperationResult<Rate>.Success("Rate deleted successfully.", existingRate));

                A.CallTo(() => _rateMapper.MapToDto(existingRate))
                    .Returns(new RateDto { RateId = 4, NightPrice = 100m });

                var result = await _ratesService.DeleteRatesAsync(deleteRatesAsync);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Rate deleted successfully.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Dto_Is_Null()
            {
                //Act
                var result = await _ratesService.DeleteRatesAsync(null);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.False(result.Data);
            }

            [Fact]
            public async Task Should_Fail_When_Rate_Does_Not_Exist()
            {
                //Arrange 
                var deleteRatesAsync = new DeleteRateDto
                {
                    RateId = 9999,
                    DeletedBy = 8
                };

                //Act

                A.CallTo(() => _deleteRateValidator.Validate(deleteRatesAsync))
                    .Returns(new ValidationResult());  // 🔍 Esto es CLAVE

                A.CallTo(() => _ratesRepository.GetByIdAsync(deleteRatesAsync.RateId))
                     .Returns(OperationResult<Rate>.Failure("Rate not found."));

                var result = await _ratesService.DeleteRatesAsync(deleteRatesAsync);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Rate not found.", result.Message);
            }

            [Fact]
            public async Task Should_Fail_When_Validation_Fails()
            {
                //Arrange
                var deleteRatesAsync = new DeleteRateDto
                {
                    RateId = 0,
                    DeletedBy = 9
                };

                //Act

                A.CallTo(() => _deleteRateValidator.Validate(deleteRatesAsync))
                    .Returns(new ValidationResult(new List<ValidationFailure> {
                new ValidationFailure("RateId", "RateId must be greater than zero.")
                    }));

                var result = await _ratesService.DeleteRatesAsync(deleteRatesAsync);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("RateId must be greater than zero.", result.Message);
            }
        }

        //GetRatesAsync
        public class GetRatesAsync : RatesServiceTest
        {
            [Fact]
            public async Task Should_Return_List_When_Exists()
            {
                //Arrange
                var rates = new List<Rate>
                {
                    new Rate {
                        RateId = 1,
                        CategoryId = 1,
                        SeasonId = 1,
                        NightPrice = 150m
                    },

                    new Rate {
                        RateId = 2,
                        CategoryId = 2,
                        SeasonId = 2,
                        NightPrice = 200m
                    }
                };

                A.CallTo(() => _ratesRepository.GetAllAsync())
                    .Returns(OperationResult<IEnumerable<Rate>>.Success("Rates retrieved.", rates));

                A.CallTo(() => _rateMapper.MapToDto(A<Rate>.Ignored))
                    .Returns(new RateDto());

                //Act
                var result = await _ratesService.GetRatesAsync();

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.True(result.Data.Any());
            }
        }

        //GetRatesByIdAsync
        public class GetRatesByIdAsync : RatesServiceTest
        {
            [Fact]
            public async Task Should_Return_Rate_When_Exists()
            {
                //Arrange
                var rate = new Rate
                {
                    RateId = 5,
                    CategoryId = 1,
                    SeasonId = 3,
                    NightPrice = 300m
                };

                A.CallTo(() => _ratesRepository.GetByIdAsync(rate.RateId))
                    .Returns(OperationResult<Rate>.Success("Rate found.", rate));

                A.CallTo(() => _rateMapper.MapToDto(rate))
                    .Returns(new RateDto { RateId = 5 });

                //Act
                var result = await _ratesService.GetRatesByIdAsync(rate.RateId);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal(5, result.Data.RateId);
            }

            [Fact]
            public async Task Should_Fail_When_Rate_Not_Found()
            {
                // Arrange
                int rateId = 9999;

                A.CallTo(() => _ratesRepository.GetByIdAsync(rateId))
                    .Returns(OperationResult<Rate>.Failure("Rate not found."));

                // Act
                var result = await _ratesService.GetRatesByIdAsync(rateId);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Rate not found.", result.Message);
            }
        }
    }
}

