using FakeItEasy;
using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Dtos.ReservationModule.ReservationService.Validators;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using SGRH.Persistence.Test.Test.ReservationModule.ReservationService.EntityBuilder;

namespace SGRH.Persistence.Test.Test.ReservationModule.ReservationService
{
    public class ReservationServiceRepositoryTest
    {
        private readonly IReservationServiceRepository _reservationServiceRepository;
        private readonly CreateReservationServiceDtoBuilder _createReservationServiceDtoBuilder = new();
        private readonly DeleteReservationServiceDtoBuilder _deleteReservationServiceDtoBuilder = new();
        public ReservationServiceRepositoryTest()
        {
            _reservationServiceRepository = A.Fake<IReservationServiceRepository>();
        }

        public class AddAsync : ReservationServiceRepositoryTest
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
                A.CallTo(() => _reservationServiceRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.AddAsync(entity);
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
                A.CallTo(() => _reservationServiceRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.AddAsync(entity);
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
                A.CallTo(() => _reservationServiceRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.AddAsync(entity);
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
                A.CallTo(() => _reservationServiceRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.AddAsync(entity);
                var expectedMessage = "ServiceId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }
        }

        public class DeleteAsync : ReservationServiceRepositoryTest
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
                A.CallTo(() => _reservationServiceRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.DeleteAsync(entity);
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
                A.CallTo(() => _reservationServiceRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.DeleteAsync(entity);
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
                A.CallTo(() => _reservationServiceRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.DeleteAsync(entity);
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
                A.CallTo(() => _reservationServiceRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.DeleteAsync(entity);
                var expectedMessage = "ServiceId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }
        }

        public class IsServiceAdded : ReservationServiceRepositoryTest
        {
            [Theory]
            [InlineData("Service is not added to reservation.", false)]
            [InlineData("Service is already added to reservation.", true)]
            public async Task IsServiceAdded_WhenDataIsValid_ShouldReturnSuccessWithMessage(string message, bool isTrue)
            {
                // Arrange
                int reservationId = 1;
                int serviceId = 1;
                var val = OperationResult<bool>.Success(message, isTrue);

                if (reservationId <= 0)
                {
                    val = OperationResult<bool>.Failure("Invalid reservation ID: it must be greater than zero.");
                }

                if (serviceId <= 0)
                {
                    val = OperationResult<bool>.Failure("Invalid service ID: it must be greater than zero.");
                }

                A.CallTo(() => _reservationServiceRepository.IsServiceAdded(reservationId, serviceId))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.IsServiceAdded(reservationId, serviceId);

                // Assert
                Assert.IsType<OperationResult<bool>>(result);
                Assert.Equal(message, result.Message);
                Assert.True(result.IsSuccess);

            }


            [Theory]
            [InlineData(-1, 1, "Invalid reservation ID: it must be greater than zero.")]
            [InlineData(1, -1, "Invalid service ID: it must be greater than zero.")]

            public async Task IsServiceAdded_WhenDataIsInvalid_ShouldReturnError(int reservationID, int serviceId, string expectedMessage)
            {
                // Arrange
                var val = OperationResult<bool>.Success("");

                if (reservationID <= 0)
                {
                    val = OperationResult<bool>.Failure("Invalid reservation ID: it must be greater than zero.");
                }

                if (serviceId <= 0)
                {
                    val = OperationResult<bool>.Failure("Invalid service ID: it must be greater than zero.");
                }

                A.CallTo(() => _reservationServiceRepository.IsServiceAdded(reservationID, serviceId))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationServiceRepository.IsServiceAdded(reservationID, serviceId);

                // Assert
                Assert.IsType<OperationResult<bool>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }


        }


    }
}
