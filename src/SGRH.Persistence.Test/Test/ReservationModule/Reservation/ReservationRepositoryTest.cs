using FakeItEasy;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Domain.Base;
using SGRH.Persistence.Test.Test.ReservationModule.Reservation.EntityBuilder;


namespace SGRH.Persistence.Test.Test.ReservationModule.Reservation
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
                var validationMessage = "";

                if (reservationId <= 0)
                {
                    validationMessage = $"Trying to find reservation with invalid id {reservationId}.";
                }

                var val = OperationResult<ReservationDto>.Failure(validationMessage);
                A.CallTo(() => _reservationRepository.GetByIdAsync(reservationId)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.GetByIdAsync(reservationId);
                var expectedMessage = $"Trying to find reservation with invalid id {reservationId}.";

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
                var dto = _createReservationDtoBuilder.WithTestValues().Build();

                var val = OperationResult<CreateReservationDto>.Success("Reservation created successfully.", dto);
                A.CallTo(() => _reservationRepository.AddAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(dto);
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

                var val = OperationResult<CreateReservationDto>.Failure("Dto cannot be null.");
                A.CallTo(() => _reservationRepository.AddAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.AddAsync(dto);
                var expectedMessage = "Dto cannot be null.";

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
                var dto = _updateReservationDtoBuilder.WithTestValues().Build();

                var val = OperationResult<UpdateReservationDto>.Success("Reservation updated successfully.", dto);
                A.CallTo(() => _reservationRepository.UpdateAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(dto);
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

                var val = OperationResult<UpdateReservationDto>.Failure("Dto cannot be null.");
                A.CallTo(() => _reservationRepository.UpdateAsync(dto)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.UpdateAsync(dto);
                var expectedMessage = "Dto cannot be null.";

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

                var val = OperationResult<DeleteReservationDto>.Success("Reservation delete successfully.");
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

                var val = OperationResult<DeleteReservationDto>.Failure("Dto cannot be null.");
                A.CallTo(() => _reservationRepository.DeleteAsync(entity)).Returns(Task.FromResult(val));

                // Act
                var result = await _reservationRepository.DeleteAsync(entity);
                var expectedMessage = "Dto cannot be null.";

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
