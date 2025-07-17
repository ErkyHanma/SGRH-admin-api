using FakeItEasy;
using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Application.Services.Hotel; 
using SGRH.Application.UseCases.Hotel.Floor;
using SGRH.Domain.Base;

namespace SGRH.Application.Test.UnitTests.Hotel
{
    public class FloorServiceTest
    {
        private readonly IFloorRepository _floorRepository = A.Fake<IFloorRepository>();
        private readonly IAppLogger<FloorService> _logger = A.Fake<IAppLogger<FloorService>>();
        private readonly IConfiguration _config = A.Fake<IConfiguration>();

        // Mocks para los casos de uso

        private readonly IMustBeUniqueValidator<int> _floorNumberMustBeUnique = A.Fake<IMustBeUniqueValidator<int>>();
        private readonly IMustNotHaveAssociationsValidator<int> _floorMustNotHaveActiveReservations = A.Fake<IMustNotHaveAssociationsValidator<int>>();

        private readonly FloorService _floorService;

        public FloorServiceTest()
        {
            // Inicializar el servicio con los mocks
            _floorService = new FloorService(
                _floorRepository,
                _logger,
                _config,
                _floorNumberMustBeUnique,
                _floorMustNotHaveActiveReservations
            );
        }

        // CreateFloor Tests
        public class CreateFloor : FloorServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                // Arrange
                var createFloorDto = new CreateFloorDto
                {
                    FloorNumber = 10,
                    Description = "Piso 10",
                    Status = "active",
                    CreatedBy = 1
                };

                // Configurar el mock del caso de uso de unicidad para que retorne éxito
                A.CallTo(() => _floorNumberMustBeUnique.ValidateCreate(createFloorDto.FloorNumber))
                    .Returns(Task.FromResult(OperationResult<string>.Success("Floor number is unique.")));

                // Configurar el mock del repositorio para que retorne éxito
                A.CallTo(() => _floorRepository.AddAsync(createFloorDto))
                    .Returns(Task.FromResult(OperationResult<CreateFloorDto>.Success("Floor created.", createFloorDto)));

                // Act
                var result = await _floorService.CreateFloor(createFloorDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                A.CallTo(() => _floorRepository.AddAsync(createFloorDto)).MustHaveHappenedOnceExactly(); // Verificar que el repositorio fue llamado
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                // Arrange
                CreateFloorDto createFloorDto = null;

                // Act
                var result = await _floorService.CreateFloor(createFloorDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("CreateFloorDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorNumber_Is_Not_Unique()
            {
                // Arrange
                var createFloorDto = new CreateFloorDto
                {
                    FloorNumber = 1, // Ya existente
                    Description = "Piso duplicado",
                    Status = "active",
                    CreatedBy = 1
                };

                // Configurar el mock del caso de uso de unicidad para que retorne fallo
                A.CallTo(() => _floorNumberMustBeUnique.ValidateCreate(createFloorDto.FloorNumber))
                    .Returns(Task.FromResult(OperationResult<string>.Failure($"Floor number {createFloorDto.FloorNumber} already exists.")));

                // Act
                var result = await _floorService.CreateFloor(createFloorDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists", result.Message);
                A.CallTo(() => _floorRepository.AddAsync(A<CreateFloorDto>._)).MustNotHaveHappened(); // El repositorio NO debe ser llamado
            }
        }

        // UpdateFloor Tests
        public class UpdateFloor : FloorServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                // Arrange
                var modifyFloorDto = new ModifyFloorDto
                {
                    FloorId = 1,
                    FloorNumber = 101,
                    Description = "Piso 101",
                    Status = "active",
                    UpdatedBy = 1
                };

                // Configurar mocks
                A.CallTo(() => _floorNumberMustBeUnique.ValidateModify(modifyFloorDto.FloorId, modifyFloorDto.FloorNumber))
                    .Returns(Task.FromResult(OperationResult<string>.Success("Floor number is unique for modification.")));

                A.CallTo(() => _floorRepository.UpdateAsync(modifyFloorDto))
                    .Returns(Task.FromResult(OperationResult<ModifyFloorDto>.Success("Floor updated.", modifyFloorDto)));

                // Act
                var result = await _floorService.UpdateFloor(modifyFloorDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                A.CallTo(() => _floorRepository.UpdateAsync(modifyFloorDto)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                // Arrange
                ModifyFloorDto modifyFloorDto = null;

                // Act
                var result = await _floorService.UpdateFloor(modifyFloorDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("ModifyFloorDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_FloorNumber_Is_Not_Unique_For_Modification()
            {
                // Arrange
                var modifyFloorDto = new ModifyFloorDto
                {
                    FloorId = 1,
                    FloorNumber = 2, // Número de otro piso existente
                    Description = "Piso a duplicar",
                    Status = "active",
                    UpdatedBy = 1
                };

                // Configurar mock de caso de uso
                A.CallTo(() => _floorNumberMustBeUnique.ValidateModify(modifyFloorDto.FloorId, modifyFloorDto.FloorNumber))
                    .Returns(Task.FromResult(OperationResult<string>.Failure($"Floor number {modifyFloorDto.FloorNumber} already exists for another floor.")));

                // Act
                var result = await _floorService.UpdateFloor(modifyFloorDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("already exists for another floor", result.Message);
                A.CallTo(() => _floorRepository.UpdateAsync(A<ModifyFloorDto>._)).MustNotHaveHappened();
            }
        }

        // DeleteFloor Tests
        public class DeleteFloor : FloorServiceTest
        {
            [Fact]
            public async Task Should_Return_Success_When_Valid()
            {
                // Arrange
                var disableFloorDto = new DisableFloorDto
                {
                    FloorId = 1,
                    UpdatedBy = 1
                };

                // Configurar mock de caso de uso
                A.CallTo(() => _floorMustNotHaveActiveReservations.Validate(disableFloorDto.FloorId))
                    .Returns(Task.FromResult(OperationResult<string>.Success("Floor has no active reservations.")));

                // Configurar mock de repositorio
                A.CallTo(() => _floorRepository.DeleteAsync(disableFloorDto))
                    .Returns(Task.FromResult(OperationResult<DisableFloorDto>.Success("Floor deleted.", disableFloorDto)));

                // Act
                var result = await _floorService.DeleteFloor(disableFloorDto);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal("Floor deleted.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Dto_Is_Null()
            {
                // Arrange
                DisableFloorDto disableFloorDto = null;

                // Act
                var result = await _floorService.DeleteFloor(disableFloorDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("DisableFloorDto is required.", result.Message);
            }

            [Fact]
            public async Task Should_Return_Error_When_Floor_Has_Active_Reservations()
            {
                // Arrange
                var disableFloorDto = new DisableFloorDto
                {
                    FloorId = 99, // ID simulado con reservas activas
                    UpdatedBy = 1
                };

                // Configurar mock de caso de uso
                A.CallTo(() => _floorMustNotHaveActiveReservations.Validate(disableFloorDto.FloorId))
                    .Returns(Task.FromResult(OperationResult<string>.Failure("Floor cannot be deleted because it has active reservations.")));

                // Act
                var result = await _floorService.DeleteFloor(disableFloorDto);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Contains("active reservations", result.Message);
                A.CallTo(() => _floorRepository.DeleteAsync(A<DisableFloorDto>._)).MustNotHaveHappened(); // El repositorio NO debe ser llamado
            }
        }

        // GetFloors Tests
        public class GetFloors : FloorServiceTest
        {
            [Fact]
            public async Task Should_Return_List_Of_Floors()
            {
                // Arrange
                var floorList = new List<FloorDto>
                {
                    new FloorDto { FloorId = 1, FloorNumber = 1, Status = "active" },
                    new FloorDto { FloorId = 2, FloorNumber = 2, Status = "active" }
                };

                A.CallTo(() => _floorRepository.GetAllAsync())
                    .Returns(Task.FromResult(OperationResult<IEnumerable<FloorDto>>.Success("Floors retrieved.", floorList)));

                // Act
                var result = await _floorService.GetFloors();

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotEmpty(result.Data);
                Assert.Equal(2, result.Data.Count());
            }

            [Fact]
            public async Task Should_Return_Error_When_Repository_Fails()
            {
                // Arrange
                A.CallTo(() => _floorRepository.GetAllAsync())
                    .Returns(Task.FromResult(OperationResult<IEnumerable<FloorDto>>.Failure("DB Error.")));

                // Act
                var result = await _floorService.GetFloors();

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("DB Error.", result.Message);
            }
        }

        // GetFloorsById Tests
        public class GetFloorsById : FloorServiceTest
        {
            [Fact]
            public async Task Should_Return_Floor_When_Found()
            {
                // Arrange
                var floor = new FloorDto { FloorId = 1, FloorNumber = 1, Status = "active" };
                A.CallTo(() => _floorRepository.GetByIdAsync(1))
                    .Returns(Task.FromResult(OperationResult<FloorDto>.Success("Floor found.", floor)));

                // Act
                var result = await _floorService.GetFloorsById(1);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Data);
                Assert.Equal(1, result.Data.FloorId);
            }

            [Fact]
            public async Task Should_Return_Error_When_Not_Found()
            {
                // Arrange
                A.CallTo(() => _floorRepository.GetByIdAsync(999))
                    .Returns(Task.FromResult(OperationResult<FloorDto>.Failure("Floor not found.")));

                // Act
                var result = await _floorService.GetFloorsById(999);

                // Assert
                Assert.False(result.IsSuccess);
                Assert.Equal("Floor not found.", result.Message);
            }
        }
    }
}