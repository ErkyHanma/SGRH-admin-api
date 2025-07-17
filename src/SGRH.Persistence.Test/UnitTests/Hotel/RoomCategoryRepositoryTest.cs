using FluentValidation;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Persistence.Test.Repositories.Hotel; 
using System.Linq; 

namespace SGRH.Persistence.Test.UnitTests.Hotel
{
    public class RoomCategoryRepositoryTest
    {
        private readonly RoomCategoryRepositoryMock _roomCategoryRepository;

        public RoomCategoryRepositoryTest()
        {
            // Setup de validadores inline para el mock
            var createValidator = new InlineValidator<CreateRoomCategoryDto>();
            createValidator.RuleFor(rc => rc.Name).NotEmpty().WithName("Name");
            createValidator.RuleFor(rc => rc.MaxCapacity).GreaterThan(0).WithName("MaxCapacity");
            createValidator.RuleFor(rc => rc.CreatedBy).GreaterThan(0).WithName("CreatedBy");

            var disableValidator = new InlineValidator<DisableRoomCategoryDto>();
            disableValidator.RuleFor(rc => rc.CategoryId).GreaterThan(0).WithName("CategoryId");
            disableValidator.RuleFor(rc => rc.UpdatedBy).GreaterThan(0).WithName("UpdatedBy");

            var modifyValidator = new InlineValidator<ModifyRoomCategoryDto>();
            modifyValidator.RuleFor(rc => rc.CategoryId).GreaterThan(0).WithName("CategoryId");
            modifyValidator.RuleFor(rc => rc.Name).NotEmpty().WithName("Name");
            modifyValidator.RuleFor(rc => rc.MaxCapacity).GreaterThan(0).WithName("MaxCapacity");
            modifyValidator.RuleFor(rc => rc.UpdatedBy).GreaterThan(0).WithName("UpdatedBy");

            _roomCategoryRepository = new RoomCategoryRepositoryMock(createValidator, disableValidator, modifyValidator);
        }

        // AddAsync Tests
        public class AddAsync : RoomCategoryRepositoryTest
        {
            [Fact]
            public async Task Should_Add_Valid_RoomCategory()
            {
                // Arrange
                var category = new CreateRoomCategoryDto
                {
                    Name = "Deluxe Suite",
                    Description = "Habitación de lujo",
                    MaxCapacity = 4,
                    Amenities = "Jacuzzi",
                    CreatedBy = 1
                };

                // Act
                var result = await _roomCategoryRepository.AddAsync(category);

                // Assert
                Assert.True(result.IsSuccess);
                var allCategories = await _roomCategoryRepository.GetAllAsync();
                Assert.Contains(allCategories.Data, rc => rc.Name == "Deluxe Suite");
            }

            [Fact]
            public async Task Should_Return_Error_When_Name_Is_Empty()
            {
                // Arrange
                var category = new CreateRoomCategoryDto
                {
                    Name = "",
                    Description = "Descripción",
                    MaxCapacity = 2,
                    Amenities = "Amenities",
                    CreatedBy = 1
                };

                // Act
                var result = await _roomCategoryRepository.AddAsync(category);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("Name", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_MaxCapacity_Is_Invalid()
            {
                // Arrange
                var category = new CreateRoomCategoryDto
                {
                    Name = "Invalid Capacity",
                    MaxCapacity = 0,
                    CreatedBy = 1
                };

                // Act
                var result = await _roomCategoryRepository.AddAsync(category);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("MaxCapacity", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomCategoryName_Already_Exists()
            {
                // Arrange

                var category = new CreateRoomCategoryDto
                {
                    Name = "Standard Single", // Duplicate
                    Description = "Duplicada",
                    MaxCapacity = 1,
                    Amenities = "Amenities",
                    CreatedBy = 1
                };

                // Act
                var result = await _roomCategoryRepository.AddAsync(category);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists", result.Message);
            }
        }

        // UpdateAsync Tests
        public class UpdateAsync : RoomCategoryRepositoryTest
        {
            [Fact]
            public async Task Should_Modify_RoomCategory_Successfully()
            {
                // Arrange
                var category = new ModifyRoomCategoryDto
                {
                    CategoryId = 1,
                    Name = "Standard Single Modified",
                    Description = "Modified Description",
                    MaxCapacity = 1,
                    Amenities = "Modified Amenities",
                    UpdatedBy = 2
                };

                // Act
                var result = await _roomCategoryRepository.UpdateAsync(category);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Room category updated successfully.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomCategory_Does_Not_Exist()
            {
                // Arrange
                var category = new ModifyRoomCategoryDto
                {
                    CategoryId = 999,
                    Name = "Non Existent",
                    MaxCapacity = 1,
                    UpdatedBy = 2
                };

                // Act
                var result = await _roomCategoryRepository.UpdateAsync(category);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Room category not found.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_CategoryId_Is_Invalid()
            {
                // Arrange
                var category = new ModifyRoomCategoryDto
                {
                    CategoryId = 0,
                    Name = "Invalid ID",
                    MaxCapacity = 1,
                    UpdatedBy = 2
                };

                // Act
                var result = await _roomCategoryRepository.UpdateAsync(category);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("CategoryId", result.Message, StringComparison.OrdinalIgnoreCase);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomCategoryName_Is_Duplicate_For_Another_Category()
            {
                // Arrange

                var categoryToUpdate = new ModifyRoomCategoryDto
                {
                    CategoryId = 1,
                    Name = "Standard Double",
                    Description = "Duplicada",
                    MaxCapacity = 1,
                    Amenities = "Amenities",
                    UpdatedBy = 1
                };

                // Act
                var result = await _roomCategoryRepository.UpdateAsync(categoryToUpdate);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists for another category", result.Message);
            }
        }

        // DeleteAsync Tests
        public class DeleteAsync : RoomCategoryRepositoryTest
        {
            [Fact]
            public async Task Should_Delete_RoomCategory_Successfully()
            {
                // Arrange
                var disableRoomCategoryDto = new DisableRoomCategoryDto
                {
                    CategoryId = 1,
                    UpdatedBy = 3
                };

                // Act
                var result = await _roomCategoryRepository.DeleteAsync(disableRoomCategoryDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Room category deleted.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomCategory_Does_Not_Exist()
            {
                // Arrange
                var disableRoomCategoryDto = new DisableRoomCategoryDto
                {
                    CategoryId = 999,
                    UpdatedBy = 3
                };

                // Act
                var result = await _roomCategoryRepository.DeleteAsync(disableRoomCategoryDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Room category not found.", result.Message);
            }
        }

        // GetByIdAsync Tests
        public class GetByIdAsync : RoomCategoryRepositoryTest
        {
            [Fact]
            public async Task Should_Return_RoomCategory_When_Found()
            {
                // Arrange (mock ya tiene categorías 1 y 2)
                int categoryId = 1;

                // Act
                var result = await _roomCategoryRepository.GetByIdAsync(categoryId);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal(categoryId, result.Data.CategoryId);
            }

            [Fact]
            public async Task Should_Return_Error_When_RoomCategory_Not_Found()
            {
                // Arrange
                int categoryId = 999;

                // Act
                var result = await _roomCategoryRepository.GetByIdAsync(categoryId);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Room category not found", result.Message);
            }
        }

        // GetAllAsync Tests
        public class GetAllAsync : RoomCategoryRepositoryTest
        {
            [Fact]
            public async Task Should_Return_All_RoomCategories()
            {
                // Arrange (mock ya tiene categorías 1 y 2)

                // Act
                var result = await _roomCategoryRepository.GetAllAsync();

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Contains(result.Data, rc => rc.CategoryId == 1);
                Assert.Contains(result.Data, rc => rc.CategoryId == 2);
                Assert.Equal(2, result.Data.Count()); 
            }
        }
    }
}