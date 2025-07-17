using FakeItEasy;
using Microsoft.Extensions.Configuration; 
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Application.Services.Hotel;
using SGRH.Application.UseCases.Hotel.RoomCategory; 
using SGRH.Domain.Base;
using System.Collections.Generic; 
using System.Linq; 

namespace SGRH.Application.Test.UnitTests.Hotel
{
    public class RoomCategoryServiceTest
    {
        private readonly IRoomCategoryRepository _roomCategoryRepository = A.Fake<IRoomCategoryRepository>();
        private readonly IRoomRepository _roomRepository = A.Fake<IRoomRepository>(); // Para RoomCategoryMustNotHaveAssociatedRooms
        private readonly IAppLogger<RoomCategoryService> _logger = A.Fake<IAppLogger<RoomCategoryService>>();
        private readonly IConfiguration _config = A.Fake<IConfiguration>();

        // Mocks para los casos de uso
       
        private readonly IMustBeUniqueValidator<string> _roomCategoryNameMustBeUnique = A.Fake<IMustBeUniqueValidator<string>>();
        private readonly IMustNotHaveAssociationsValidator<int> _roomCategoryMustNotHaveAssociatedRooms = A.Fake<IMustNotHaveAssociationsValidator<int>>();


        private readonly RoomCategoryService _roomCategoryService;

        public RoomCategoryServiceTest()
        {
            
            _roomCategoryService = new RoomCategoryService(
                _roomCategoryRepository,
                _logger,
                _config,
                _roomCategoryNameMustBeUnique,
                _roomCategoryMustNotHaveAssociatedRooms
            );
        }

        // CreateRoomCategory Tests
        public class CreateRoomCategory : RoomCategoryServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                // Arrange
                var createCategoryDto = new CreateRoomCategoryDto
                {
                    Name = "Luxury Suite",
                    MaxCapacity = 4,
                    CreatedBy = 1
                };

                // Configurar el mock del caso de uso de unicidad
                A.CallTo(() => _roomCategoryNameMustBeUnique.ValidateCreate(createCategoryDto.Name))
                    .Returns(Task.FromResult(OperationResult<string>.Success("Category name is unique.")));

                // Configurar el mock del repositorio
                A.CallTo(() => _roomCategoryRepository.AddAsync(createCategoryDto))
                    .Returns(Task.FromResult(OperationResult<CreateRoomCategoryDto>.Success("Category created.", createCategoryDto)));

                // Act
                var result = await _roomCategoryService.CreateRoomCategory(createCategoryDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                A.CallTo(() => _roomCategoryRepository.AddAsync(createCategoryDto)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                // Arrange
                CreateRoomCategoryDto createCategoryDto = null;

                // Act
                var result = await _roomCategoryService.CreateRoomCategory(createCategoryDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("CreateRoomCategoryDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_CategoryName_Is_Not_Unique()
            {
                // Arrange
                var createCategoryDto = new CreateRoomCategoryDto
                {
                    Name = "Standard Single", // Ya existente
                    MaxCapacity = 1,
                    CreatedBy = 1
                };

                // Configurar el mock del caso de uso de unicidad
                A.CallTo(() => _roomCategoryNameMustBeUnique.ValidateCreate(createCategoryDto.Name))
                    .Returns(Task.FromResult(OperationResult<string>.Failure($"Room category name '{createCategoryDto.Name}' already exists.")));

                // Act
                var result = await _roomCategoryService.CreateRoomCategory(createCategoryDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists", result.Message);
                A.CallTo(() => _roomCategoryRepository.AddAsync(A<CreateRoomCategoryDto>._)).MustNotHaveHappened();
            }
        }

        // UpdateRoomCategory Tests
        public class UpdateRoomCategory : RoomCategoryServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                // Arrange
                var modifyCategoryDto = new ModifyRoomCategoryDto
                {
                    CategoryId = 1,
                    Name = "Standard Single Updated",
                    MaxCapacity = 1,
                    UpdatedBy = 1
                };

                // Configurar mocks
                A.CallTo(() => _roomCategoryNameMustBeUnique.ValidateModify(modifyCategoryDto.CategoryId, modifyCategoryDto.Name))
                    .Returns(Task.FromResult(OperationResult<string>.Success("Category name is unique for modification.")));

                A.CallTo(() => _roomCategoryRepository.UpdateAsync(modifyCategoryDto))
                    .Returns(Task.FromResult(OperationResult<ModifyRoomCategoryDto>.Success("Category updated.", modifyCategoryDto)));

                // Act
                var result = await _roomCategoryService.UpdateRoomCategory(modifyCategoryDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                A.CallTo(() => _roomCategoryRepository.UpdateAsync(modifyCategoryDto)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                // Arrange
                ModifyRoomCategoryDto modifyCategoryDto = null;

                // Act
                var result = await _roomCategoryService.UpdateRoomCategory(modifyCategoryDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("ModifyRoomCategoryDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_CategoryName_Is_Not_Unique_For_Modification()
            {
                // Arrange
                var modifyCategoryDto = new ModifyRoomCategoryDto
                {
                    CategoryId = 1,
                    Name = "Standard Double", // Nombre de otra categoría existente
                    MaxCapacity = 1,
                    UpdatedBy = 1
                };

                // Configurar mock de caso de uso
                A.CallTo(() => _roomCategoryNameMustBeUnique.ValidateModify(modifyCategoryDto.CategoryId, modifyCategoryDto.Name))
                    .Returns(Task.FromResult(OperationResult<string>.Failure($"Room category name '{modifyCategoryDto.Name}' already exists for another category.")));

                // Act
                var result = await _roomCategoryService.UpdateRoomCategory(modifyCategoryDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists for another category", result.Message);
                A.CallTo(() => _roomCategoryRepository.UpdateAsync(A<ModifyRoomCategoryDto>._)).MustNotHaveHappened();
            }
        }

        // DeleteRoomCategory Tests
        public class DeleteRoomCategory : RoomCategoryServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                // Arrange
                var disableCategoryDto = new DisableRoomCategoryDto
                {
                    CategoryId = 1,
                    UpdatedBy = 1
                };

                // Configurar mock de caso de uso
                A.CallTo(() => _roomCategoryMustNotHaveAssociatedRooms.Validate(disableCategoryDto.CategoryId))
                    .Returns(Task.FromResult(OperationResult<string>.Success("Category has no associated rooms.")));

                // Configurar mock de repositorio
                A.CallTo(() => _roomCategoryRepository.DeleteAsync(disableCategoryDto))
                    .Returns(Task.FromResult(OperationResult<DisableRoomCategoryDto>.Success("Category deleted.", disableCategoryDto)));

                // Act
                var result = await _roomCategoryService.DeleteRoomCategory(disableCategoryDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Category deleted.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                // Arrange
                DisableRoomCategoryDto disableCategoryDto = null;

                // Act
                var result = await _roomCategoryService.DeleteRoomCategory(disableCategoryDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("DisableRoomCategoryDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Category_Has_Associated_Rooms()
            {
                // Arrange
                var disableCategoryDto = new DisableRoomCategoryDto
                {
                    CategoryId = 1,
                    UpdatedBy = 1
                };

                // Configurar mock de caso de uso
                A.CallTo(() => _roomCategoryMustNotHaveAssociatedRooms.Validate(disableCategoryDto.CategoryId))
                    .Returns(Task.FromResult(OperationResult<string>.Failure("Category cannot be deleted because it has associated active rooms.")));

                // Act
                var result = await _roomCategoryService.DeleteRoomCategory(disableCategoryDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("associated active rooms", result.Message);
                A.CallTo(() => _roomCategoryRepository.DeleteAsync(A<DisableRoomCategoryDto>._)).MustNotHaveHappened();
            }
        }

        // GetRoomCategories Tests
        public class GetRoomCategories : RoomCategoryServiceTest
        {
            [Fact]
            public async Task Should_Return_List_Of_RoomCategories()
            {
                // Arrange
                var categoryList = new List<RoomCategoryDto>
                {
                    new RoomCategoryDto { CategoryId = 1, Name = "Standard Single", MaxCapacity = 1, CreatedBy = 1, CreatedAt = DateTime.Now },
                    new RoomCategoryDto { CategoryId = 2, Name = "Standard Double", MaxCapacity = 2, CreatedBy = 1, CreatedAt = DateTime.Now }
                };

                A.CallTo(() => _roomCategoryRepository.GetAllAsync())
                    .Returns(Task.FromResult(OperationResult<IEnumerable<RoomCategoryDto>>.Success("Categories retrieved.", categoryList)));

                // Act
                var result = await _roomCategoryService.GetRoomCategories();

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotEmpty(result.Data);
                Assert.Equal(2, result.Data.Count());
            }

            [Fact]
            public async Task Should_Return_Error_When_Repository_Fails()
            {
                // Arrange
                A.CallTo(() => _roomCategoryRepository.GetAllAsync())
                    .Returns(Task.FromResult(OperationResult<IEnumerable<RoomCategoryDto>>.Failure("DB Error.")));

                // Act
                var result = await _roomCategoryService.GetRoomCategories();

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("DB Error.", result.Message);
            }
        }

        // GetRoomCategoryById Tests
        public class GetRoomCategoryById : RoomCategoryServiceTest
        {
            [Fact]
            public async Task Should_Return_RoomCategory_When_Found()
            {
                // Arrange
                var category = new RoomCategoryDto { CategoryId = 1, Name = "Standard Single", MaxCapacity = 1, CreatedBy = 1, CreatedAt = DateTime.Now };
                A.CallTo(() => _roomCategoryRepository.GetByIdAsync(1))
                    .Returns(Task.FromResult(OperationResult<RoomCategoryDto>.Success("Category found.", category)));

                // Act
                var result = await _roomCategoryService.GetRoomCategoryById(1);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal(1, result.Data.CategoryId);
            }

            [Fact]
            public async Task Should_Return_Error_When_Not_Found()
            {
                // Arrange
                A.CallTo(() => _roomCategoryRepository.GetByIdAsync(999))
                    .Returns(Task.FromResult(OperationResult<RoomCategoryDto>.Failure("Category not found.")));

                // Act
                var result = await _roomCategoryService.GetRoomCategoryById(999);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Category not found.", result.Message);
            }
        }
    }
}