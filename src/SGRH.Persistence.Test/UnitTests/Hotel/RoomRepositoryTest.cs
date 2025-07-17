using FluentValidation;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Domain.Entities.Hotel;
using SGRH.Persistence.Test.Repositories.Hotel;

/** Estas pruebas son las pruebas unitarias del repositorio room, usan un mock falso manual.
    En este caso, las hice antes de impelementar FakeItEasy, es una forma de hacerlo sin usar frameworks **/

namespace SGRH.Persistence.Test.UnitTests.Hotel
{
    public class RoomRepositoryTest
    {
        private readonly RoomRepositoryMock _roomRepository;
        public RoomRepositoryTest()
        {
            var createValidator = new InlineValidator<CreateRoomDto>();
            createValidator.RuleFor(r => r.RoomNumber).NotEmpty().WithName("RoomNumber");
            createValidator.RuleFor(r => r.CategoryId).GreaterThan(0).WithName("CategoryId");
            createValidator.RuleFor(r => r.FloorId).GreaterThan(0).WithName("FloorId");
            createValidator.RuleFor(r => r.Status).Must(s => new[] { "available", "maintenance", "occupied" }.Contains(s));
            createValidator.RuleFor(r => r.CreatedBy).GreaterThan(0).WithName("CreatedBy");

            var disableValidator = new InlineValidator<DisableRoomDto>();
            disableValidator.RuleFor(r => r.RoomId).GreaterThan(0).WithName("RoomId");
            disableValidator.RuleFor(r => r.UpdatedBy).GreaterThan(0).WithName("UpdatedBy");

            var modifyValidator = new InlineValidator<ModifyRoomDto>();
            modifyValidator.RuleFor(r => r.RoomId).GreaterThan(0).WithName("RoomId");
            modifyValidator.RuleFor(r => r.RoomNumber).NotEmpty().WithName("RoomNumber");
            modifyValidator.RuleFor(r => r.CategoryId).GreaterThan(0).WithName("CategoryId");
            modifyValidator.RuleFor(r => r.FloorId).GreaterThan(0).WithName("FloorId");
            modifyValidator.RuleFor(r => r.Status).Must(s => new[] { "available", "maintenance", "occupied" }.Contains(s));
            modifyValidator.RuleFor(r => r.UpdatedBy).GreaterThan(0).WithName("UpdatedBy"); ;

            _roomRepository = new RoomRepositoryMock(createValidator, disableValidator, modifyValidator);
        }

        //AddAsync
        public class AddAsync : RoomRepositoryTest
        {
            [Fact]
            public async Task Should_Add_Valid_Room()
            {
                //Arrange

                var room = new CreateRoomDto
                {
                    RoomNumber = "101",
                    CategoryId = 1,
                    FloorId = 2,
                    CreatedBy = 7,
                    Status = "available"
                };

                //Act

                var result = await _roomRepository.AddAsync(room);

                //Assert
                Assert.True(result.IsSuccess);
                var all = await _roomRepository.GetAllAsync();
                Assert.Single(all.Data);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomNumber_Is_Empty()
            {
                //Arrange

                var room = new CreateRoomDto
                {
                    RoomNumber = "",
                    CategoryId = 1,
                    FloorId = 2,
                    CreatedBy = 7,
                    Status = "available"
                };

                //Act
                var result = await _roomRepository.AddAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                var all = await _roomRepository.GetAllAsync();
                Assert.Empty(all.Data);
            }

            [Fact]
            public async Task Should_Return_Error_When_CategoryId_Is_Invalid()
            {
                //Arrange

                var room = new CreateRoomDto
                {
                    RoomNumber = "102",
                    CategoryId = 0,
                    FloorId = 1,
                    CreatedBy = 7,
                    Status = "available"
                };

                //Act
                var result = await _roomRepository.AddAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("CategoryId", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorId_Is_Invalid()
            {
                //Arrange
                var room = new CreateRoomDto
                {
                    RoomNumber = "103",
                    CategoryId = 0,
                    FloorId = -11,
                    CreatedBy = 7,
                    Status = "maintenance"
                };

                //Act
                var result = await _roomRepository.AddAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("Floor", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_Status_Is_Invalid()
            {
                //Arrange

                var room = new CreateRoomDto
                {
                    RoomNumber = "333",
                    CategoryId = 2,
                    FloorId = 4,
                    Status = "Not a valid status!!!!!!!!!",
                    CreatedBy = 7
                };

                //Act

                var result = await _roomRepository.AddAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("Status", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_CreatedBy_Is_Zero()
            {
                //Arrange
                var room = new CreateRoomDto
                {
                    RoomNumber = "104",
                    CategoryId = 2,
                    FloorId = 1,
                    CreatedBy = 0,
                    Status = "available"
                };

                //Act
                var result = await _roomRepository.AddAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("CreatedBy", result.Message, StringComparison.OrdinalIgnoreCase);
            }

        }

        //UpdateAsync 
        public class UpdateAsync : RoomRepositoryTest
        {

            [Fact]
            public async Task Should_Modify_Room_Successfully()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 1,
                    RoomNumber = "207",
                    CategoryId = 1,
                    FloorId = 6,
                    Status = "occupied",
                    UpdatedBy = 7
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Room updated successfully.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Room_Does_Not_Exist()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 9999,
                    RoomNumber = "208",
                    CategoryId = 3,
                    FloorId = 2,
                    Status = "occupied",
                    UpdatedBy = 7
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Room not found", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomId_Is_Invalid()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 0,
                    RoomNumber = "209",
                    CategoryId = 5,
                    FloorId = 1,
                    Status = "available",
                    UpdatedBy = 7
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("RoomId", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomNumber_Is_Empty()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 4,
                    RoomNumber = "",
                    CategoryId = 1,
                    FloorId = 6,
                    Status = "maintenance",
                    UpdatedBy = 7
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("RoomNumber", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_CategoryId_Is_Invalid()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 5,
                    RoomNumber = "331",
                    CategoryId = 0,
                    FloorId = 6,
                    Status = "maintenance",
                    UpdatedBy = 7
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("CategoryId", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorId_Is_Invalid()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 6,
                    RoomNumber = "332",
                    CategoryId = 6,
                    FloorId = 0,
                    Status = "available",
                    UpdatedBy = 7
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("FloorId", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_Status_Is_Invalid()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 7,
                    RoomNumber = "333",
                    CategoryId = 2,
                    FloorId = 4,
                    Status = "This is NOT a valid status.",
                    UpdatedBy = 0
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("Status", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_UpdatedBy_Is_Zero()
            {
                //Arrange

                var room = new ModifyRoomDto
                {
                    RoomId = 7,
                    RoomNumber = "333",
                    CategoryId = 2,
                    FloorId = 4,
                    Status = "available",
                    UpdatedBy = 0
                };

                //Act

                var result = await _roomRepository.UpdateAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("UpdatedBy", result.Message, StringComparison.OrdinalIgnoreCase);
            }
        }

        //DeleteAsync
        public class DeleteAsync : RoomRepositoryTest
        {
            [Fact]
            public async Task Should_Delete_Room_Successfully()
            {
                //Arrange

                var room = new DisableRoomDto
                {
                    RoomId = 1,
                    UpdatedBy = 7,
                };

                //Act

                var result = await _roomRepository.DeleteAsync(room);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Room deleted.", result.Message);
            }
            [Fact]
            public async Task Should_Return_Error_When_Room_Does_Not_Exist()
            {
                //Arrange

                var room = new DisableRoomDto
                {
                    RoomId = 9999,
                    UpdatedBy = 7,
                };

                //Act

                var result = await _roomRepository.DeleteAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Room not found.", result.Message);
            }
            [Fact]
            public async Task Should_Return_Error_When_RoomId_Is_Invalid()
            {
                //Arrange

                var room = new DisableRoomDto
                {
                    RoomId = 0,
                    UpdatedBy = 7,
                };

                //Act

                var result = await _roomRepository.DeleteAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("RoomId", result.Message, StringComparison.OrdinalIgnoreCase);
            }
            [Fact]
            public async Task Should_Return_Error_When_UpdatedBy_Is_Invalid()
            {
                //Arrange

                var room = new DisableRoomDto
                {
                    RoomId = 7,
                    UpdatedBy = 0,
                };

                //Act

                var result = await _roomRepository.DeleteAsync(room);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("UpdatedBy", result.Message, StringComparison.OrdinalIgnoreCase);
            }
        }

        //GetByIdAsync
        public class GetByIdAsync : RoomRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Room()
            {
                //Arrange

                var dto = new CreateRoomDto
                {
                    RoomNumber = "201",
                    CategoryId = 2,
                    FloorId = 3,
                    CreatedBy = 7
                };

                //Act

                var added = await _roomRepository.AddAsync(dto);
                var result = await _roomRepository.GetByIdAsync(1);


                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("201", result.Data.RoomNumber);
            }

            [Fact]
            public async Task Should_Return_Error_When_Room_Does_Not_Exist()
            {
                //Act

                var result = await _roomRepository.GetByIdAsync(999);

                //Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Room not found", result.Message);
            }
        }

        //GetAll
        public class GetAll : RoomRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Existing_Rooms()
            {
                var room1 = new Room { RoomNumber = "203", CategoryId = 1, FloorId = 1, CreatedBy = 7 };
                var room2 = new Room { RoomNumber = "204", CategoryId = 1, FloorId = 2, CreatedBy = 7 };
                var room3 = new Room { RoomNumber = "205", CategoryId = 2, FloorId = 3, CreatedBy = 7 };

                //Act

                var result = await _roomRepository.GetAllAsync();

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Rooms retireved sucessfully.", result.Message);
            }
        }
    }
}