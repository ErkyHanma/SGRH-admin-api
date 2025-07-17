using FluentValidation;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Persistence.Test.Repositories.Hotel; 

namespace SGRH.Persistence.Test.UnitTests.Hotel
{
    public class FloorRepositoryTest
    {
        private readonly FloorRepositoryMock _floorRepository;

        public FloorRepositoryTest()
        {
            // Setup de validadores inline para el mock
            var createValidator = new InlineValidator<CreateFloorDto>();
            createValidator.RuleFor(f => f.FloorNumber).GreaterThan(0).WithName("FloorNumber");
            createValidator.RuleFor(f => f.Status).Must(s => new[] { "active", "inactive", "maintenance" }.Contains(s)).WithName("Status");
            createValidator.RuleFor(f => f.CreatedBy).GreaterThan(0).WithName("CreatedBy");

            var disableValidator = new InlineValidator<DisableFloorDto>();
            disableValidator.RuleFor(f => f.FloorId).GreaterThan(0).WithName("FloorId");
            disableValidator.RuleFor(f => f.UpdatedBy).GreaterThan(0).WithName("UpdatedBy");

            var modifyValidator = new InlineValidator<ModifyFloorDto>();
            modifyValidator.RuleFor(f => f.FloorId).GreaterThan(0).WithName("FloorId");
            modifyValidator.RuleFor(f => f.FloorNumber).GreaterThan(0).WithName("FloorNumber");
            modifyValidator.RuleFor(f => f.Status).Must(s => new[] { "active", "inactive", "maintenance" }.Contains(s)).WithName("Status");
            modifyValidator.RuleFor(f => f.UpdatedBy).GreaterThan(0).WithName("UpdatedBy");

            _floorRepository = new FloorRepositoryMock(createValidator, disableValidator, modifyValidator);
        }

        // AddAsync Tests
        public class AddAsync : FloorRepositoryTest
        {
            [Fact]
            public async Task Should_Add_Valid_Floor()
            {
                // Arrange
                var floor = new CreateFloorDto
                {
                    FloorNumber = 3,
                    Description = "Tercer Piso",
                    Status = "active",
                    CreatedBy = 1
                };

                // Act
                var result = await _floorRepository.AddAsync(floor);

                // Assert
                Assert.True(result.IsSuccess);
                var allFloors = await _floorRepository.GetAllAsync();
                Assert.Contains(allFloors.Data, f => f.FloorNumber == 3);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorNumber_Is_Invalid()
            {
                // Arrange
                var floor = new CreateFloorDto
                {
                    FloorNumber = 0,
                    Description = "Piso Invalido",
                    Status = "active",
                    CreatedBy = 1
                };

                // Act
                var result = await _floorRepository.AddAsync(floor);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("FloorNumber", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_Status_Is_Invalid()
            {
                // Arrange
                var floor = new CreateFloorDto
                {
                    FloorNumber = 4,
                    Description = "Cuarto Piso",
                    Status = "invalid_status",
                    CreatedBy = 1
                };

                // Act
                var result = await _floorRepository.AddAsync(floor);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("Status", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorNumber_Already_Exists()
            {
                // Arrange
                // FloorRepositoryMock ya tiene el 1 y 2
                var floor = new CreateFloorDto
                {
                    FloorNumber = 1, // Already exists
                    Description = "Piso duplicado",
                    Status = "active",
                    CreatedBy = 1
                };

                // Act
                var result = await _floorRepository.AddAsync(floor);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists", result.Message);
            }
        }

        // UpdateAsync Tests
        public class UpdateAsync : FloorRepositoryTest
        {
            [Fact]
            public async Task Should_Modify_Floor_Successfully()
            {
                // Arrange
                var floor = new ModifyFloorDto
                {
                    FloorId = 1,
                    FloorNumber = 10,
                    Description = "Piso Diez Modificado",
                    Status = "maintenance",
                    UpdatedBy = 2
                };

                // Act
                var result = await _floorRepository.UpdateAsync(floor);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Floor updated successfully.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Floor_Does_Not_Exist()
            {
                // Arrange
                var floor = new ModifyFloorDto
                {
                    FloorId = 999,
                    FloorNumber = 99,
                    Description = "Piso Inexistente",
                    Status = "active",
                    UpdatedBy = 2
                };

                // Act
                var result = await _floorRepository.UpdateAsync(floor);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Floor not found.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorId_Is_Invalid()
            {
                // Arrange
                var floor = new ModifyFloorDto
                {
                    FloorId = 0,
                    FloorNumber = 5,
                    Description = "Piso ID Invalido",
                    Status = "active",
                    UpdatedBy = 2
                };

                // Act
                var result = await _floorRepository.UpdateAsync(floor);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("FloorId", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorNumber_Is_Duplicate_For_Another_Floor()
            {
                // Arrange
                // FloorRepositoryMock tiene el 1 y 2. Intentamos cambiar el 1 para que tenga el número 2.
                var floorToUpdate = new ModifyFloorDto
                {
                    FloorId = 1,
                    FloorNumber = 2, // Duplicate of existing floor 2
                    Description = "Piso duplicado",
                    Status = "active",
                    UpdatedBy = 1
                };

                // Act
                var result = await _floorRepository.UpdateAsync(floorToUpdate);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists for another floor", result.Message);
            }
        }

        // DeleteAsync Tests
        public class DeleteAsync : FloorRepositoryTest
        {
            [Fact]
            public async Task Should_Delete_Floor_Successfully()
            {
                // Arrange
                var disableFloorDto = new DisableFloorDto
                {
                    FloorId = 1,
                    UpdatedBy = 3
                };

                // Act
                var result = await _floorRepository.DeleteAsync(disableFloorDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Floor deleted.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Floor_Does_Not_Exist()
            {
                // Arrange
                var disableFloorDto = new DisableFloorDto
                {
                    FloorId = 999,
                    UpdatedBy = 3
                };

                // Act
                var result = await _floorRepository.DeleteAsync(disableFloorDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Floor not found.", result.Message);
            }
        }

        // GetByIdAsync Tests
        public class GetByIdAsync : FloorRepositoryTest
        {
            [Fact]
            public async Task Should_Return_Floor_When_Found()
            {
                // Arrange (el mock ya tiene pisos 1 y 2 activos)
                int floorId = 1;

                // Act
                var result = await _floorRepository.GetByIdAsync(floorId);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal(floorId, result.Data.FloorId);
            }

            [Fact]
            public async Task Should_Return_Error_When_Floor_Not_Found()
            {
                // Arrange
                int floorId = 999;

                // Act
                var result = await _floorRepository.GetByIdAsync(floorId);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Floor not found.", result.Message);
            }
        }

        // GetAllAsync Tests
        public class GetAllAsync : FloorRepositoryTest
        {
            [Fact]
            public async Task Should_Return_All_Active_Floors()
            {
                // Arrange (el mock ya tiene pisos 1 y 2 activos)
                await _floorRepository.AddAsync(new CreateFloorDto { FloorNumber = 3, Description = "Piso a desactivar", Status = "active", CreatedBy = 1 });
                await _floorRepository.DeleteAsync(new DisableFloorDto { FloorId = 3, UpdatedBy = 1 });

                // Act
                var result = await _floorRepository.GetAllAsync();

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Contains(result.Data, f => f.FloorId == 1);
                Assert.Contains(result.Data, f => f.FloorId == 2);
                Assert.DoesNotContain(result.Data, f => f.FloorId == 3); 
                Assert.Equal(2, result.Data.Count()); 
            }
        }
    }
}