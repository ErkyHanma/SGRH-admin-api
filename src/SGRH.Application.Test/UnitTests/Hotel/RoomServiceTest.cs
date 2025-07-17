using FakeItEasy;
using SGRH.Application.Common.Logging;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Services.Hotel;
using SGRH.Application.UseCases.Hotel.Room;
using Microsoft.Extensions.Configuration;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Domain.Base;
using SGRH.Application.Interfaces.UseCases;

/** Se usa FakeItEasy para simular dependencias externas (repositorios, validadores).
 El objetivo es probar la logica de RoomService **/

namespace SGRH.Application.Test.UnitTests.Hotel
{
    public class RoomServiceTest
    {
        private readonly IRoomRepository _roomRepository = A.Fake<IRoomRepository>();
        private readonly IMustExistValidator<int> _categoryValidator = A.Fake<IMustExistValidator<int>>();
        private readonly IMustExistValidator<int> _floorValidator;
        private readonly IMustNotBeOccupied<RoomDto> _roomMustNotBeOccupied;
        private readonly IAppLogger<RoomService> _logger = A.Fake<IAppLogger<RoomService>>();
        private readonly IConfiguration _config = A.Fake<IConfiguration>();

        private readonly RoomService _roomService;
        public RoomServiceTest()
        {
            _categoryValidator = A.Fake<IMustExistValidator<int>>();
            _floorValidator = A.Fake<IMustExistValidator<int>>();
            _roomMustNotBeOccupied = A.Fake<IMustNotBeOccupied<RoomDto>>();

            _roomService = new RoomService(
            _roomRepository,
            _logger,
            _config,
            _roomMustNotBeOccupied,
            _categoryValidator,
            _floorValidator);
        }

        // CreateRoom
        public class CreateRoom : RoomServiceTest
        {
            [Fact]
            public async Task Should_Return_Sucess_When_Valid()
            {
                // Arrange
                var createRoomDto = new CreateRoomDto
                {
                    RoomNumber = "101",
                    CategoryId = 1,
                    FloorId = 1,
                    Status = "available",
                    CreatedBy = 7
                };

                A.CallTo(() => _categoryValidator.Validate(createRoomDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _floorValidator.Validate(createRoomDto.FloorId))
                    .Returns(OperationResult<string>.Success("Floor exists."));

                A.CallTo(() => _roomRepository.AddAsync(createRoomDto))
                    .Returns(OperationResult<CreateRoomDto>.Success("Created", createRoomDto));

                // Act
                var result = await _roomService.CreateRoom(createRoomDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                // Arrange
                CreateRoomDto createRoomDto = null;

                // Act
                var result = await _roomService.CreateRoom(createRoomDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("CreateRoomDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Category_Not_Exist()
            {
                // Arrange
                var createRoomDto = new CreateRoomDto
                {
                    RoomNumber = "102",
                    CategoryId = 9999,
                    FloorId = 3,
                    Status = "available",
                    CreatedBy = 7
                };

                // Act
                A.CallTo(() => _categoryValidator.Validate(createRoomDto.CategoryId))
                    .Returns(OperationResult<string>.Failure("CategoryId 9999 does not exist."));

                var result = await _roomService.CreateRoom(createRoomDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("CategoryId 9999 does not exist.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Floor_Not_Exist()
            {
                // Arrange
                var createRoomDto = new CreateRoomDto
                {
                    RoomNumber = "103",
                    CategoryId = 1,
                    FloorId = 9999,
                    Status = "occupied",
                    CreatedBy = 8
                };

                // Act
                A.CallTo(() => _categoryValidator.Validate(createRoomDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _floorValidator.Validate(createRoomDto.FloorId))
                    .Returns(OperationResult<string>.Failure("FloorId 9999 does not exist."));

                var result = await _roomService.CreateRoom(createRoomDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("FloorId 9999 does not exist.", result.Message);
            }
        }

        // UpdateRoom
        public class UpdateRoom : RoomServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                //Arrange
                var modifyRoomDto = new ModifyRoomDto
                {
                    RoomId = 1,
                    RoomNumber = "201",
                    CategoryId = 1,
                    FloorId = 1,
                    Status = "available",
                    UpdatedBy = 10
                };

                var existingRoom = new RoomDto
                {
                    RoomId = 1,
                    RoomNumber = "Old",
                    CategoryId = 1,
                    FloorId = 1,
                    Status = "occupied",
                    CreatedAt = DateTime.Now,
                    CreatedBy = 5
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(modifyRoomDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _floorValidator.Validate(modifyRoomDto.FloorId))
                    .Returns(OperationResult<string>.Success("Floor exists."));

                A.CallTo(() => _roomRepository.GetByIdAsync(modifyRoomDto.RoomId))
                    .Returns(OperationResult<RoomDto>.Success("Found", existingRoom));

                A.CallTo(() => _roomMustNotBeOccupied.Validate(existingRoom))
                    .Returns(OperationResult<string>.Success("Room is not occupied."));

                A.CallTo(() => _roomRepository.UpdateAsync(modifyRoomDto))
                    .Returns(OperationResult<ModifyRoomDto>.Success("Updated", modifyRoomDto));

                var result = await _roomService.UpdateRoom(modifyRoomDto);

                //Assert
                Assert.True(result.IsSuccess);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                //Arrange
                ModifyRoomDto modifyRoomDto = null;

                //Act
                var result = await _roomService.UpdateRoom(modifyRoomDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("ModifyRoomDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Category_Not_Exists()
            {
                //Arrange 

                var modifyRoomDto = new ModifyRoomDto
                {
                    RoomId = 1,
                    CategoryId = 9999,
                    FloorId = 4
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(modifyRoomDto.CategoryId))
                    .Returns(OperationResult<string>.Failure("CategoryId 9999 does not exist."));

                var result = await _roomService.UpdateRoom(modifyRoomDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("CategoryId 9999 does not exist.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Floor_Not_Exists()
            {
                var modifyRoomDto = new ModifyRoomDto
                {

                    RoomId = 1,
                    CategoryId = 3,
                    FloorId = 9999
                };

                A.CallTo(() => _categoryValidator.Validate(modifyRoomDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _floorValidator.Validate(modifyRoomDto.FloorId))
                    .Returns(OperationResult<string>.Failure("FloorId 9999 does not exist."));

                var result = await _roomService.UpdateRoom(modifyRoomDto);

                Assert.False(result.IsSuccess);
                Assert.Equal("FloorId 9999 does not exist.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Room_Is_Occupied()
            {
                //Arrange
                var modifyRoomDto = new ModifyRoomDto
                {

                    RoomId = 1,
                    CategoryId = 3,
                    FloorId = 3
                };

                var room = new RoomDto
                {
                    RoomId = 1,
                    RoomNumber = "105",
                    Status = "occupied",
                    CategoryId = 1,
                    FloorId = 1,
                    CreatedBy = 5,
                    CreatedAt = DateTime.Now
                };

                //Act
                A.CallTo(() => _categoryValidator.Validate(modifyRoomDto.CategoryId))
                    .Returns(OperationResult<string>.Success("Category exists."));

                A.CallTo(() => _floorValidator.Validate(modifyRoomDto.FloorId))
                    .Returns(OperationResult<string>.Success("Floor exists."));

                A.CallTo(() => _roomRepository.GetByIdAsync(modifyRoomDto.RoomId))
                    .Returns(OperationResult<RoomDto>.Success("Found", room));

                A.CallTo(() => _roomMustNotBeOccupied.Validate(room))
                    .Returns(OperationResult<string>.Failure("The room is occupied."));

                var result = await _roomService.UpdateRoom(modifyRoomDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("The room is occupied and cannot be modified.", result.Message);
            }
        }

        // DeleteRoom
        public class DeleteRoom : RoomServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                //Arrange
                var disableRoomDto = new DisableRoomDto
                {

                    RoomId = 1,
                    UpdatedBy = 5
                };

                var room = new RoomDto
                {

                    RoomId = 1,
                    Status = "available"
                };

                //Act
                A.CallTo(() => _roomRepository.GetByIdAsync(disableRoomDto.RoomId))
                    .Returns(OperationResult<RoomDto>.Success("Found", room));

                A.CallTo(() => _roomMustNotBeOccupied.Validate(room))
                    .Returns(OperationResult<string>.Success("Room is available."));

                A.CallTo(() => _roomRepository.DeleteAsync(disableRoomDto))
                    .Returns(OperationResult<DisableRoomDto>.Success("Deleted", disableRoomDto));

                var result = await _roomService.DeleteRoom(disableRoomDto);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Deleted", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                //Arrange
                DisableRoomDto disableRoomDto = null;

                //Act
                var result = await _roomService.DeleteRoom(disableRoomDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("DisableRoomDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Room_Not_Found()
            {
                //Arrange
                var disableRoomDto = new DisableRoomDto
                {

                    RoomId = 999,
                    UpdatedBy = 5
                };

                //Act
                A.CallTo(() => _roomRepository.GetByIdAsync(disableRoomDto.RoomId))
                    .Returns(OperationResult<RoomDto>.Failure("Room not found"));

                var result = await _roomService.DeleteRoom(disableRoomDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("RoomId 999 does not exist.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Room_Is_Occupied()
            {
                //Arrange
                var disableRoomDto = new DisableRoomDto
                {
                    RoomId = 2,
                    UpdatedBy = 5
                };

                var room = new RoomDto
                {
                    RoomId = 2,
                    Status = "occupied"
                };

                //Act
                A.CallTo(() => _roomRepository.GetByIdAsync(disableRoomDto.RoomId))
                    .Returns(OperationResult<RoomDto>.Success("Found", room));

                A.CallTo(() => _roomMustNotBeOccupied.Validate(room))
                    .Returns(OperationResult<string>.Failure("The room is occupied."));

                var result = await _roomService.DeleteRoom(disableRoomDto);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("The room is occupied and cannot be modified.", result.Message);
            }
        }

        //GetRooms
        public class GetRooms : RoomServiceTest
        {
            [Fact]
            public async Task GetRooms_Should_Return_List_Of_Rooms()
            {
                //Arrange
                var list = new List<RoomDto>
                {
                    new RoomDto {

                        RoomId = 1,
                        RoomNumber = "101",
                        CategoryId = 1,
                        FloorId = 1,
                        Status = "available",
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now
                    }
                };

                //Act

                A.CallTo(() => _roomRepository.GetAllAsync())
                    .Returns(OperationResult<IEnumerable<RoomDto>>.Success("ok", list));

                var result = await _roomService.GetRooms();

                //Assert
                Assert.True(result.IsSuccess);
                Assert.NotEmpty(result.Data);
            }
        }

        //GetRoomsById
        public class GetRoomsById : RoomServiceTest
        {
            [Fact]
            public async Task Should_Return_Room_When_Found()
            {
                //Arrange
                var room = new RoomDto
                {

                    RoomId = 1,
                    RoomNumber = "101",
                    CategoryId = 1,
                    FloorId = 1,
                    Status = "available",
                    CreatedBy = 1,
                    CreatedAt = DateTime.Now
                };

                //Act
                A.CallTo(() => _roomRepository.GetByIdAsync(1))
                    .Returns(OperationResult<RoomDto>.Success("Found", room));

                var result = await _roomService.GetRoomsById(1);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal(1, result.Data.RoomId);
            }

            [Fact]
            public async Task Should_Return_Error_When_Not_Found()
            {
                //Act
                A.CallTo(() => _roomRepository.GetByIdAsync(999))
                    .Returns(OperationResult<RoomDto>.Failure("Room not found"));

                var result = await _roomService.GetRoomsById(999);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Room not found", result.Message);
            }
        }
    }
}
