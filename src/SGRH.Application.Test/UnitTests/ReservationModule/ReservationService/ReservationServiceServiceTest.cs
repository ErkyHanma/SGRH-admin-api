using FakeItEasy;
using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Dtos.ReservationModule.ReservationService.Validators;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Application.Test.Test.ReservationModule.ReservationService.EntityBuilder;
using SGRH.Domain.Base;

namespace SGRH.Application.Test.Test.ReservationModule.ReservationService
{
    public class ReservationServiceServiceTest
    {
        private readonly IReservationServiceService _reservationServiceService;
        private readonly CreateReservationServiceDtoBuilder _createReservationServiceDtoBuilder = new();
        private readonly DeleteReservationServiceDtoBuilder _deleteReservationServiceDtoBuilder = new();
        public ReservationServiceServiceTest()
        {
            _reservationServiceService = A.Fake<IReservationServiceService>();
        }

        public class AddAsync : ReservationServiceServiceTest
        {
            [Fact]
            public async Task AddAsync_WhenCreateReservationServiceDtoIsValid_ShouldReturnCreateReservationServiceDto()
            {
                // Arrange
                var entity = _createReservationServiceDtoBuilder.WithTestValues().Build();

                var createReservationServiceDtoValidator = new CreateReservationServiceDtoValidator();
                var validationResult = createReservationServiceDtoValidator.Validate(entity);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Service added successfully.";
                }

                var val = OperationResult<CreateReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.AddReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.AddReservationServiceAsync(entity);
                var expectedMessage = "Service added successfully.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async Task AddAsync_WhenDtoIsNull_ShouldReturnError()
            {
                // Arrange
                CreateReservationServiceDto entity = null;

                var createReservationServiceDtoValidator = new CreateReservationServiceDtoValidator();
                var validationResult = createReservationServiceDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.AddReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.AddReservationServiceAsync(entity);
                var expectedMessage = "Dto cannot be null.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async Task AddAsync_WhenReservationIdInvalid_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationServiceDtoBuilder.WithTestValues().WithReservationId(-1).Build();

                var createReservationServiceDtoValidator = new CreateReservationServiceDtoValidator();
                var validationResult = createReservationServiceDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.AddReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.AddReservationServiceAsync(entity);
                var expectedMessage = "ReservationId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async Task AddAsync_WhenServiceIdInvalid_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationServiceDtoBuilder.WithTestValues().WithServiceId(-2).Build();

                var createReservationServiceDtoValidator = new CreateReservationServiceDtoValidator();
                var validationResult = createReservationServiceDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.AddReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.AddReservationServiceAsync(entity);
                var expectedMessage = "ServiceId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }
        }

        public class DeleteAsync : ReservationServiceServiceTest
        {
            [Fact]
            public async Task DeleteAsync_WhenCreateReservationServiceDtoIsValid_ShouldReturnCreateReservationServiceDto()
            {
                // Arrange
                var entity = _deleteReservationServiceDtoBuilder.WithTestValues().Build();

                var deleteReservationServiceDtoValidator = new DeleteReservationServiceDtoValidator();
                var validationResult = deleteReservationServiceDtoValidator.Validate(entity);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Service deleted successfully.";
                }

                var val = OperationResult<DeleteReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.DeleteReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.DeleteReservationServiceAsync(entity);
                var expectedMessage = "Service deleted successfully.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async Task DeleteAsync_WhenDtoIsNull_ShouldReturnError()
            {
                // Arrange
                DeleteReservationServiceDto entity = null;

                var deleteReservationServiceDtoValidator = new DeleteReservationServiceDtoValidator();
                var validationResult = deleteReservationServiceDtoValidator.Validate(entity);

                var val = OperationResult<DeleteReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.DeleteReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.DeleteReservationServiceAsync(entity);
                var expectedMessage = "Dto cannot be null.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async Task DeleteAsync_WhenReservationIdInvalid_ShouldReturnError()
            {
                // Arrange
                var entity = _deleteReservationServiceDtoBuilder.WithTestValues().WithReservationId(-1).Build();

                var deleteReservationServiceDtoValidator = new DeleteReservationServiceDtoValidator();
                var validationResult = deleteReservationServiceDtoValidator.Validate(entity);

                var val = OperationResult<DeleteReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.DeleteReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.DeleteReservationServiceAsync(entity);
                var expectedMessage = "ReservationId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async Task DeleteAsync_WhenServiceIdInvalid_ShouldReturnError()
            {
                // Arrange
                var entity = _deleteReservationServiceDtoBuilder.WithTestValues().WithServiceId(-2).Build();

                var deleteReservationServiceDtoValidator = new DeleteReservationServiceDtoValidator();
                var validationResult = deleteReservationServiceDtoValidator.Validate(entity);

                var val = OperationResult<DeleteReservationServiceDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationServiceService.DeleteReservationServiceAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceService.DeleteReservationServiceAsync(entity);
                var expectedMessage = "ServiceId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }
        }




    }
}
