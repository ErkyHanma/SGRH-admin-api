using FakeItEasy;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Dtos.ReservationModule.Reservation.Validators;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using SGRH.Persistence.Test.Test.ReservationModule.EntityBuilder;


namespace SGRH.Persistence.Test.Test.ReservationModule
{
    public class ReservationRepositoryTest
    {
        private readonly CreateReservationDtoBuilder _createReservationDtoBuilder = new();
        private readonly UpdateReservationDtoBuilder _updateReservationDtoBuilder = new();
        private readonly DeleteReservationDtoBuilder _deleteReservationDtoBuilder = new();
        private readonly IReservationRepository _reservationRepository;

        public ReservationRepositoryTest()
        {

            _reservationRepository = A.Fake<IReservationRepository>();
        }

        public class GetByIdAsync : ReservationRepositoryTest
        {
            [Fact]

            public async Task GetByIdAsync_WhenReservationIdIsValid_ShouldReturnReservationDto()
            {
                // Arrange
                var reservationId = 1;

                var val = OperationResult<ReservationDto>.Success("Reservation retrieved successfully.");
                A.CallTo(() => _reservationRepository.GetByIdAsync(reservationId)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.GetByIdAsync(reservationId);
                var expectedMessage = "Reservation retrieved successfully.";

                // Assert
                Assert.IsType<OperationResult<ReservationDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async void GetByIdAsync_WhenReservationIdIsInvalid_ShouldReturnError()
            {
                // Arrange
                var reservationId = -1;
                var validationMessage = "All fields validated";

                if (reservationId <= 0)
                {
                    validationMessage = "Invalid reservation ID";
                }

                var val = OperationResult<ReservationDto>.Failure(validationMessage);
                A.CallTo(() => _reservationRepository.GetByIdAsync(reservationId)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.GetByIdAsync(reservationId);
                var expectedMessage = "Invalid reservation ID";

                // Assert
                Assert.IsType<OperationResult<ReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);




            }
        }

        public class AddAsync : ReservationRepositoryTest
        {

            [Fact]
            public async void AddAsync_WhenEntityDtoIsValid_ShouldReturnCreateReservationDto()
            {
                // Arrange
                var entity = _createReservationDtoBuilder.WithTestValues().Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Reservation created successfully.";
                }

                var val = OperationResult<CreateReservationDto>.Success(validationResult.Message, entity);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "Reservation created successfully.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);



            }

            [Fact]
            public async void AddAsync_WhenEntityDtoIsNull_ShouldReturnError()
            {
                // Arrange
                CreateReservationDto entity = null;

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "Dto cannot be null.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenClientIdIsNullOrNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationDtoBuilder.WithTestValues().WithClientId(-2).Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "ClientId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenRoomIdIsNullOrNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationDtoBuilder.WithTestValues().WithRoomId(-2).Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);


                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "RoomId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenStartDateIsGreaterOrEqualToEndDate_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithStartDate(DateTime.Today)
                    .WithEndDate(DateTime.Today)
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "StartDate must be before EndDate.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenStatusIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithStatus("")
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "Status is required.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenGuestCountIsLessOrEqualToZero_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithGuestCount(0)
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "GuestCount must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenPaymentAmountIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithPaymentAmount(-100)
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(entity);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.AddAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(entity);
                var expectedMessage = "PaymentAmount cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

        }

        public class UpdateAsync : ReservationRepositoryTest
        {

            [Fact]
            public async void UpdateAsync_WhenEntityDtoIsValid_ShouldReturnUpdateReservationDto()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder.WithTestValues().Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Reservation updated successfully.";
                }

                var val = OperationResult<UpdateReservationDto>.Success(validationResult.Message, entity);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "Reservation updated successfully.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenEntityDtoIsNull_ShouldReturnError()
            {
                // Arrange
                UpdateReservationDto entity = null;

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "Dto cannot be null.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenClientIdIsNullOrNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder.WithTestValues().WithClientId(-3).Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "ClientId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenRoomIdIsNullOrNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder.WithTestValues().WithRoomId(-2).Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "RoomId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenStartDateIsGreaterOrEqualToEndDate_ShouldReturnError()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithStartDate(DateTime.Today)
                    .WithEndDate(DateTime.Today)
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "StartDate must be before EndDate.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenStatusIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithStatus("")
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "Status is required.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenGuestCountIsLessOrEqualToZero_ShouldReturnError()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithGuestCount(-2)
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "GuestCount must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenPaymentAmountIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithPaymentAmount(-100)
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "PaymentAmount cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenReservationIdIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithReservationId(-1)
                    .Build();


                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(entity);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.UpdateAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(entity);
                var expectedMessage = "ReservationId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }



        }

        public class DeleteAsync : ReservationRepositoryTest
        {
            [Fact]
            public async void DeleteAsync_WhenEntityDtoIsValid_ShouldReturnDeleteReservationDto()
            {
                // Arrange
                var entity = _deleteReservationDtoBuilder.WithTestValues().Build();

                var deleteReservationDtoValidator = new DeleteReservationDtoValidator();
                var validationResult = deleteReservationDtoValidator.Validate(entity);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Reservation delete successfully.";
                }

                var val = OperationResult<DeleteReservationDto>.Success(validationResult.Message);
                A.CallTo(() => _reservationRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.DeleteAsync(entity);
                var expectedMessage = "Reservation delete successfully.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenEntityDtoIsNull_ShouldReturnError()
            {
                // Arrange
                DeleteReservationDto entity = null;

                var deleteReservationDtoValidator = new DeleteReservationDtoValidator();
                var validationResult = deleteReservationDtoValidator.Validate(entity);

                var val = OperationResult<DeleteReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.DeleteAsync(entity);
                var expectedMessage = "Dto cannot be null.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenReservationIdIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _deleteReservationDtoBuilder
                    .WithTestValues()
                    .WithReservationId(-2)
                    .Build();


                var deleteReservationDtoValidator = new DeleteReservationDtoValidator();
                var validationResult = deleteReservationDtoValidator.Validate(entity);

                var val = OperationResult<DeleteReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.DeleteAsync(entity);
                var expectedMessage = "ReservationId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }


        }

        public class CheckRoomAvailability : ReservationRepositoryTest
        {
            [Fact]
            public async Task CheckRoomAvailability_WhenRoomIsAvailable_ShouldReturnCheckRoomAvailableDto()
            {
                // Arrange
                var roomId = 1;
                var startDate = DateTime.Now;
                var endDate = DateTime.Now.AddDays(1);
                var val = OperationResult<CheckRoomAvailabilityResultDto>.Success("The Room is available!");


                if (roomId <= 0)
                {
                    val = OperationResult<CheckRoomAvailabilityResultDto>.Failure("Room ID must be greater than zero.");
                }

                if (startDate >= endDate)
                {
                    val = OperationResult<CheckRoomAvailabilityResultDto>.Failure("Start date must be before end date.");
                }


                A.CallTo(() => _reservationRepository.CheckAvailability(
                    roomId, startDate, endDate))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.CheckAvailability(roomId, startDate, endDate);
                var expectedMessage = "The Room is available!";

                // Assert
                Assert.IsType<OperationResult<CheckRoomAvailabilityResultDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async Task CheckRoomAvailability_WhenRoomIdIsNegative_ShouldReturnError()
            {
                // Arrange
                var roomId = -1;
                var startDate = DateTime.Now;
                var endDate = DateTime.Now.AddDays(1);
                var validationMessage = "";

                if (roomId <= 0)
                {
                    validationMessage = "Room ID must be greater than zero.";
                }

                var val = OperationResult<CheckRoomAvailabilityResultDto>.Failure(validationMessage);

                A.CallTo(() => _reservationRepository.CheckAvailability(
                    roomId, startDate, endDate))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.CheckAvailability(roomId, startDate, endDate);
                var expectedMessage = "Room ID must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<CheckRoomAvailabilityResultDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async Task CheckRoomAvailability_WhenStartDateIsGreaterOrEqualToEndDate_ShouldReturnError()
            {
                // Arrange
                var startDate = DateTime.Now.AddDays(2);
                var endDate = DateTime.Now;
                var validationMessage = "";

                if (startDate >= endDate)
                {
                    validationMessage = "Start date must be before end date.";
                }

                var val = OperationResult<CheckRoomAvailabilityResultDto>.Failure(validationMessage);

                A.CallTo(() => _reservationRepository.CheckAvailability(
                    1, startDate, endDate))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.CheckAvailability(1, startDate, endDate);
                var expectedMessage = "Start date must be before end date.";

                // Assert
                Assert.IsType<OperationResult<CheckRoomAvailabilityResultDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);



            }


        }

        public class ExistsAsync : ReservationRepositoryTest
        {
            [Fact]
            public async void ExistsAsync_WhenReservationExists_ShouldReturnTrue()
            {
                // Arrange
                var reservationId = 1;
                var val = OperationResult<bool>.Success("Reservation exists.", true);
                A.CallTo(() => _reservationRepository.ExistsAsync(reservationId)).Returns(Task.FromResult(val));


                // Act
                var result = await _reservationRepository.ExistsAsync(reservationId);
                var expectedMessage = "Reservation exists.";


                // Assert
                Assert.IsType<OperationResult<bool>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
                Assert.True(result.Data);
            }

            [Fact]
            public async void ExistsAsync_WhenReservationDoesNotExist_ShouldReturnFalse()
            {
                // Arrange
                var reservationId = 2;
                var val = OperationResult<bool>.Success("Reservation does not exist.", false);
                A.CallTo(() => _reservationRepository.ExistsAsync(reservationId)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.ExistsAsync(reservationId);
                var expectedMessage = "Reservation does not exist.";

                // Assert
                Assert.IsType<OperationResult<bool>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
                Assert.False(result.Data);
            }

            [Fact]
            public async Task ExistsAsync_WhenrRservationIdIsNegative_ShouldReturnError()
            {
                // Arrange
                var reservationId = -1;
                var validationMessage = "All fields validated!";

                if (reservationId <= 0)
                {
                    validationMessage = "Invalid reservation ID it must be greater than zero.";
                }

                var val = OperationResult<bool>.Failure(validationMessage);

                A.CallTo(() => _reservationRepository.ExistsAsync(reservationId))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.ExistsAsync(reservationId);
                var expectedMessage = "Invalid reservation ID it must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<bool>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }
        }
    }
}
