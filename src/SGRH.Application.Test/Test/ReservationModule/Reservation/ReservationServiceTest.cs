using FakeItEasy;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Dtos.ReservationModule.Reservation.Validators;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder;
using SGRH.Domain.Base;

namespace SGRH.Application.Test.Test.ReservationModule.Reservation
{
    public class ReservationServiceTest
    {

        private readonly CreateReservationDtoBuilder _createReservationDtoBuilder = new();
        private readonly UpdateReservationDtoBuilder _updateReservationDtoBuilder = new();
        private readonly DeleteReservationDtoBuilder _deleteReservationDtoBuilder = new();
        private readonly IReservationService _reservationService;

        public ReservationServiceTest()
        {
            _reservationService = A.Fake<IReservationService>();
        }

        public class GetByIdAsync : ReservationServiceTest
        {
            [Fact]

            public async Task GetByIdAsync_WhenReservationIdIsValid_ShouldReturnReservationDto()
            {
                // Arrange
                var reservationId = 1;

                var val = OperationResult<ReservationDto>.Success("Reservation retrieved successfully.");
                A.CallTo(() => _reservationService.GetReservationByIdAsync(reservationId)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.GetReservationByIdAsync(reservationId);
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
                A.CallTo(() => _reservationService.GetReservationByIdAsync(reservationId)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.GetReservationByIdAsync(reservationId);
                var expectedMessage = "Invalid reservation ID";

                // Assert
                Assert.IsType<OperationResult<ReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);




            }
        }

        public class AddAsync : ReservationServiceTest
        {

            [Fact]
            public async void AddAsync_WhenEntityDtoIsValid_ShouldReturnCreateReservationDto()
            {
                // Arrange
                var dto = _createReservationDtoBuilder.WithTestValues().Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Reservation created successfully.";
                }

                var val = OperationResult<CreateReservationDto>.Success(validationResult.Message, dto);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
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
                CreateReservationDto dto = null;

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
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
                var dto = _createReservationDtoBuilder.WithTestValues().WithClientId(-2).Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
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
                var dto = _createReservationDtoBuilder.WithTestValues().WithRoomId(-2).Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);


                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
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
                var dto = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithStartDate(DateTime.Today)
                    .WithEndDate(DateTime.Today)
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
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
                var dto = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithStatus("")
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
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
                var dto = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithGuestCount(0)
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
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
                var dto = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithPaymentAmount(-100)
                    .Build();

                var createReservationDtoValidator = new CreateReservationDtoValidator();
                var validationResult = createReservationDtoValidator.Validate(dto);

                var val = OperationResult<CreateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
                var expectedMessage = "PaymentAmount cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenDateIsLessThanCurrentDate_ShouldReturnError()
            {
                // Arrange
                var dto = _createReservationDtoBuilder
                    .WithTestValues()
                    .WithStartDate(new DateTime(2024, 12, 05))
                    .WithEndDate(new DateTime(2024, 12, 07))
                    .Build();

                var validationMessage = "All field validated!";

                if (dto.StartDate <= DateTime.Now || dto.EndDate <= DateTime.Now)
                {
                    validationMessage = "Reservation cannot be in the past";
                }

                var val = OperationResult<CreateReservationDto>.Failure(validationMessage);
                A.CallTo(() => _reservationService.AddReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.AddReservationAsync(dto);
                var expectedMessage = "Reservation cannot be in the past";

                // Assert
                Assert.IsType<OperationResult<CreateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }


        }

        public class UpdateAsync : ReservationServiceTest
        {

            [Fact]
            public async void UpdateAsync_WhenEntityDtoIsValid_ShouldReturnUpdateReservationDto()
            {
                // Arrange
                var dto = _updateReservationDtoBuilder.WithTestValues().Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Reservation updated successfully.";
                }

                var val = OperationResult<UpdateReservationDto>.Success(validationResult.Message, dto);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                UpdateReservationDto dto = null;

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                var dto = _updateReservationDtoBuilder.WithTestValues().WithClientId(-3).Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                var dto = _updateReservationDtoBuilder.WithTestValues().WithRoomId(-2).Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                var dto = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithStartDate(DateTime.Today)
                    .WithEndDate(DateTime.Today)
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                var dto = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithStatus("")
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                var dto = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithGuestCount(-2)
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                var dto = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithPaymentAmount(-100)
                    .Build();

                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
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
                var dto = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithReservationId(-1)
                    .Build();


                var updateReservationDtoValidator = new UpdateReservationDtoValidator();
                var validationResult = updateReservationDtoValidator.Validate(dto);

                var val = OperationResult<UpdateReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
                var expectedMessage = "ReservationId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenDateIsLessThanCurrentDate_ShouldReturnError()
            {
                // Arrange
                var dto = _updateReservationDtoBuilder
                    .WithTestValues()
                    .WithStartDate(new DateTime(2024, 12, 5))
                    .WithEndDate(new DateTime(2024, 12, 7))
                    .Build();

                var validationMessage = "All field validated!";

                if (dto.StartDate <= DateTime.Now || dto.EndDate <= DateTime.Now)
                {
                    validationMessage = "Reservation cannot be in the past";
                }

                var val = OperationResult<UpdateReservationDto>.Failure(validationMessage);
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
                var expectedMessage = "Reservation cannot be in the past";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenReservationDoesNotExist_ShouldReturnError()
            {
                // Arrange
                var dto = _updateReservationDtoBuilder.WithTestValues().Build();
                var val = OperationResult<UpdateReservationDto>.Failure($"The reservation with ID {dto.ReservationId} does not exists");
                A.CallTo(() => _reservationService.UpdateReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.UpdateReservationAsync(dto);
                var expectedMessage = $"The reservation with ID {dto.ReservationId} does not exists";

                // Assert
                Assert.IsType<OperationResult<UpdateReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

        }

        public class DeleteAsync : ReservationServiceTest
        {
            [Fact]
            public async void DeleteAsync_WhenEntityDtoIsValid_ShouldReturnDeleteReservationDto()
            {
                // Arrange
                var dto = _deleteReservationDtoBuilder.WithTestValues().Build();

                var deleteReservationDtoValidator = new DeleteReservationDtoValidator();
                var validationResult = deleteReservationDtoValidator.Validate(dto);

                if (validationResult.IsSuccess)
                {
                    validationResult.Message = "Reservation delete successfully.";
                }

                var val = OperationResult<DeleteReservationDto>.Success(validationResult.Message);
                A.CallTo(() => _reservationService.DeleteReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.DeleteReservationAsync(dto);
                var expectedMessage = "Reservation delete successfully.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsyncc_WhenReservationDoesNotExist_ShouldReturnError()
            {
                // Arrange
                var dto = _deleteReservationDtoBuilder.WithTestValues().Build();
                var val = OperationResult<DeleteReservationDto>.Failure($"The reservation with ID {dto.ReservationId} does not exists");
                A.CallTo(() => _reservationService.DeleteReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.DeleteReservationAsync(dto);
                var expectedMessage = $"The reservation with ID {dto.ReservationId} does not exists";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenEntityDtoIsNull_ShouldReturnError()
            {
                // Arrange
                DeleteReservationDto dto = null;

                var deleteReservationDtoValidator = new DeleteReservationDtoValidator();
                var validationResult = deleteReservationDtoValidator.Validate(dto);

                var val = OperationResult<DeleteReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.DeleteReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.DeleteReservationAsync(dto);
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
                var dto = _deleteReservationDtoBuilder
                    .WithTestValues()
                    .WithReservationId(-2)
                    .Build();


                var deleteReservationDtoValidator = new DeleteReservationDtoValidator();
                var validationResult = deleteReservationDtoValidator.Validate(dto);

                var val = OperationResult<DeleteReservationDto>.Failure(validationResult.Message);
                A.CallTo(() => _reservationService.DeleteReservationAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.DeleteReservationAsync(dto);
                var expectedMessage = "ReservationId must be greater than zero.";

                // Assert
                Assert.IsType<OperationResult<DeleteReservationDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }


        }

        public class CheckRoomAvailability : ReservationServiceTest
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


                A.CallTo(() => _reservationService.CheckAvailability(
                    roomId, startDate, endDate))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.CheckAvailability(roomId, startDate, endDate);
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

                A.CallTo(() => _reservationService.CheckAvailability(
                    roomId, startDate, endDate))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.CheckAvailability(roomId, startDate, endDate);
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

                A.CallTo(() => _reservationService.CheckAvailability(
                    1, startDate, endDate))
                    .Returns(Task.FromResult(val));

                // Act
                var result = await _reservationService.CheckAvailability(1, startDate, endDate);
                var expectedMessage = "Start date must be before end date.";

                // Assert
                Assert.IsType<OperationResult<CheckRoomAvailabilityResultDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);



            }


        }



    }
}
