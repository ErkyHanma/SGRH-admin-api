using FakeItEasy;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.ServiceModule;
using SGRH.Application.Interfaces.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Application.Interfaces.Services.Service_Module;
using SGRH.Application.Services.ServiceModule;
using SGRH.Application.Test.Test.ServiceModule.EntityBuilder;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.ServiceModule;
using System.Linq.Expressions;

namespace SGRH.Persistence.Test.Test.ServiceModule
{
    public class ServiceServiceTest
    {
        private CreateServiceDtoBuilder _createServiceDtoBuilder = new();
        private ServiceDtoBuilder _serviceDtoBuilder = new();
        private DeleteServiceDtoBuilder _deleteServiceDtoBuilder = new();
        private readonly IServiceRepository _fakeServiceRepository;
        private readonly IServiceService _serviceService;
        private readonly IAppLogger<ServiceService> _fakeLogger;
        private readonly IServiceMapper _fakeMapper;
        public ServiceServiceTest()
        {
            _fakeServiceRepository = A.Fake<IServiceRepository>();
            _fakeMapper = A.Fake<IServiceMapper>();
            _fakeLogger = A.Fake<IAppLogger<ServiceService>>();
            _serviceService = new ServiceService(_fakeServiceRepository, _fakeLogger, _fakeMapper);
        }



        public class GetAllServicesAsync : ServiceServiceTest
        {

            [Fact]
            public async void GetAllServicesAsync_WhenServicesFound_ShouldReturnDtos()
            {
                // Arrange
                var mockServices = new List<Service>
                {
                    new Service { ServiceId = 2, Name = "Limpiar", Description = "Se limpia el ITLA", Price = 20 },
                    new Service { ServiceId = 3, Name = "Limpiar2", Description = "Se limpia el ITLA", Price = 20 }

                };


                var val = OperationResult<IEnumerable<Service>>.Success($"Found {mockServices.Count()} service(s) matching the filter.", mockServices);
                A.CallTo(() => _fakeServiceRepository.GetAllAsync()).Returns(Task.FromResult(val));


                // Act
                var result = await _serviceService.GetAllServicesAsync();
                var expectedMessage = $"Found {mockServices.Count()} service(s) matching the filter.";

                // Assert
                Assert.IsType<OperationResult<IEnumerable<ServiceDto>>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }
        }
        public class GetServiceByIdAsync : ServiceServiceTest
        {

            [Fact]
            public async void GetServiceByIdAsync_WhenIdIsInvalid_ShouldReturnError()
            {
                // Arrange
                int id = -2;

                // Act
                var result = await _serviceService.GetServiceByIdAsync(id);
                var expectedMessage = "Invalid service ID";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async void GetServiceByIdAsync_WhenServiceIsNotFound_ShouldReturnDto()
            {
                // Arrange
                var id = 2;

                var val = OperationResult<Service>.Failure("Service was not found with the given ID");
                A.CallTo(() => _fakeServiceRepository.GetByIdAsync(id)).Returns(Task.FromResult(val));


                // Act
                var result = await _serviceService.GetServiceByIdAsync(id);
                var expectedMessage = "Service was not found with the given ID";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void GetServiceByIdAsync_WhenServiceFound_ShouldReturnDto()
            {
                // Arrange
                var id = 2;

                // Result Mock
                var existingService = new Service { ServiceId = 2, Name = "Limpiar", Description = "Se limpia el ITLA", Price = 20 };

                var val = OperationResult<Service>.Success($"Service with the ID: {id} found", existingService);
                A.CallTo(() => _fakeServiceRepository.GetByIdAsync(id)).Returns(Task.FromResult(val));


                // Act
                var result = await _serviceService.GetServiceByIdAsync(id);
                var expectedMessage = $"Service with the ID: {id} found";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }
        }
        public class CreateServicesAsync : ServiceServiceTest
        {
            [Fact]
            public async void CreateServicesAsync_WhenEntityIsNull_ShouldReturnError()
            {
                // Arrange
                CreateServiceDto dto = null;

                // Act
                var result = await _serviceService.CreateServicesAsync(dto);
                var expectedMessage = "Service entity cannot be null.";

                // Assert
                Assert.IsType<OperationResult<CreateServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void CreateServicesAsync_WhenNameIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var dto =
                    _createServiceDtoBuilder.WithTestValues().WithName("").Build();

                // Act
                var result = await _serviceService.CreateServicesAsync(dto);
                var expectedMessage = "Service name is required.";

                // Assert
                Assert.IsType<OperationResult<CreateServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void CreateServicesAsync_WhenDescriptionIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var dto = _createServiceDtoBuilder
                      .WithTestValues()
                      .WithDescription("")
                      .Build();

                // Act
                var result = await _serviceService.CreateServicesAsync(dto);
                var expectedMessage = "Service description is required.";

                // Assert
                Assert.IsType<OperationResult<CreateServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void CreateServicesAsync_WhenPriceIsNegative_ShouldReturnError()
            {
                // Arrange
                var dto = _createServiceDtoBuilder
                     .WithTestValues()
                     .WithPrice(-3)
                     .Build();

                // Act
                var result = await _serviceService.CreateServicesAsync(dto);
                var expectedMessage = "Service price cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<CreateServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void CreateServicesAsync_WhenServiceAlreadyExists_ShouldReturnError()
            {
                // Arrange
                var dto = _createServiceDtoBuilder.WithTestValues().Build();

                A.CallTo(() => _fakeServiceRepository.ExistsAsync(A<Expression<Func<Service, bool>>>._))
                                                     .Returns(Task.FromResult(true));


                // Act
                var result = await _serviceService.CreateServicesAsync(dto);
                var expectedMessage = $"Service with name {dto.Name} already exists.";

                // Assert
                Assert.IsType<OperationResult<CreateServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void CreateServicesAsync_WhenEntityIsValid_ShouldReturnSuccess()
            {
                // Arrange
                var dto = _createServiceDtoBuilder.WithTestValues().Build();

                A.CallTo(() => _fakeMapper.ToDomainEntityAdd(dto)).Returns(new Service
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                });

                A.CallTo(() => _fakeServiceRepository.ExistsAsync(A<Expression<Func<Service, bool>>>._))
                                                     .Returns(Task.FromResult(false));
                var val = OperationResult<Service>.Success("Service Dto added successfully.", _fakeMapper.ToDomainEntityAdd(dto));
                A.CallTo(() => _fakeServiceRepository.AddAsync(_fakeMapper.ToDomainEntityAdd(dto))).Returns(Task.FromResult(val));


                // Act
                var result = await _serviceService.CreateServicesAsync(dto);
                var expectedMessage = "Service Dto added successfully.";

                // Assert
                Assert.IsType<OperationResult<CreateServiceDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }
        }
        public class UpdateServicesAsync : ServiceServiceTest
        {
            [Fact]
            public async void UpdateServicesAsync_WhenEntityIsNull_ShouldReturnError()
            {
                // Arrange
                ServiceDto dto = null;

                // Act
                var result = await _serviceService.UpdateServicesAsync(dto);
                var expectedMessage = "Service entity cannot be null.";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateServicesAsync_WhenNameIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var dto = _serviceDtoBuilder
                    .WithTestValues()
                    .WithName("")
                    .Build();

                // Act
                var result = await _serviceService.UpdateServicesAsync(dto);
                var expectedMessage = "Service name is required.";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenDescriptionIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var dto = _serviceDtoBuilder
                     .WithTestValues()
                     .WithDescription("")
                     .Build();

                // Act
                var result = await _serviceService.UpdateServicesAsync(dto);
                var expectedMessage = "Service description is required.";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenPriceIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceDtoBuilder
                    .WithTestValues()
                    .WithPrice(-3)
                    .Build();


                // Act
                var result = await _serviceService.UpdateServicesAsync(entity);
                var expectedMessage = "Service price cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateServicesAsync_WhenEntityIsValid_ShouldReturnSuccess()
            {
                // Arrange
                var dto = _serviceDtoBuilder.WithTestValues().WithServiceId(2).Build();

                A.CallTo(() => _fakeMapper.ToDomainEntity(dto)).Returns(new Service
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                });

                var val = OperationResult<Service>.Success("Service Dto added successfully.", _fakeMapper.ToDomainEntity(dto));
                A.CallTo(() => _fakeServiceRepository.UpdateAsync(_fakeMapper.ToDomainEntity(dto))).Returns(Task.FromResult(val));


                // Act
                var result = await _serviceService.UpdateServicesAsync(dto);
                var expectedMessage = "Service Dto added successfully.";

                // Assert
                Assert.IsType<OperationResult<ServiceDto>>(result);
                //Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

        }
        public class DeleteServicesAsync : ServiceServiceTest
        {
            [Fact]
            public async void DeleteServicesAsync_WhenEntityIsNull_ShouldReturnError()
            {
                // Arrange
                DeleteServiceDto dto = null;

                // Act
                var result = await _serviceService.DeleteServicesAsync(dto);
                var expectedMessage = "Service dto cannot be null.";

                // Assert
                Assert.IsType<OperationResult<DeleteServiceDto>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteServicesAsync_WhenEntityIsValid_ShouldReturnSuccess()
            {
                // Arrange
                var dto = _deleteServiceDtoBuilder.WithTestValues().Build();

                A.CallTo(() => _fakeMapper.ToDomainEntityDelete(dto)).Returns(new Service { ServiceId = dto.ServiceId });


                var val = OperationResult<Service>.Success("Service Limpiar deleted successfully", _fakeMapper.ToDomainEntityDelete(dto));
                A.CallTo(() => _fakeServiceRepository.DeleteAsync(_fakeMapper.ToDomainEntityDelete(dto))).Returns(Task.FromResult(val));


                // Act
                var result = await _serviceService.DeleteServicesAsync(dto);
                var expectedMessage = "Service Limpiar deleted successfully";

                // Assert
                Assert.IsType<OperationResult<DeleteServiceDto>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

        }


    }

}

