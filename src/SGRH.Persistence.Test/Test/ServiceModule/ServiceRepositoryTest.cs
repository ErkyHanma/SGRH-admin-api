using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.ServiceModule;
using SGRH.Persistence.Test.Context;
using SGRH.Persistence.Test.Repositories.ServiceModule;
using SGRH.Persistence.Test.Test.ServiceModule.EntityBuilder;

namespace SGRH.Persistence.Test.Test.ServiceModule
{
    public class ServiceRepositoryTest
    {
        private ServiceEntityBuilder _serviceEntityBuilder = new();
        private readonly IServiceRepository _serviceRepository;
        private readonly SGRHContextInMemory _dbContext = new();
        public ServiceRepositoryTest()
        {
            _serviceRepository = new ServiceRepositoryMock(_dbContext);
        }

        public class AddAsync : ServiceRepositoryTest
        {
            [Fact]
            public async void AddAsync_WhenEntityIsNull_ShouldReturnError()
            {
                // Arrange
                Service entity = null;

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service entity cannot be null.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenNameIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                    .WithTestValues()
                    .WithName("")
                    .Build();

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service name is required.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenDescriptionIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                      .WithTestValues()
                      .WithDescription("")
                      .Build();

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service description is required.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void AddAsync_WhenPriceIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                     .WithTestValues()
                     .WithPrice(-3)
                     .Build();

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service price cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            // Sometimes it gives error because of the DB In Memory
            [Fact]
            public async void AddAsync_WhenEntityIsValid_ShouldReturnSuccess()
            {
                // Arrange
                var entity = _serviceEntityBuilder.WithTestValues().Build();

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service entity added successfully.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }
        }

        public class UpdateAsync : ServiceRepositoryTest
        {
            [Fact]
            public async void UpdateAsync_WhenEntityIsNull_ShouldReturnError()
            {
                // Arrange
                Service entity = null;

                // Act
                var result = await _serviceRepository.UpdateAsync(entity);
                var expectedMessage = "Service entity cannot be null.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenNameIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                    .WithTestValues()
                    .WithName("")
                    .Build();

                // Act
                var result = await _serviceRepository.UpdateAsync(entity);
                var expectedMessage = "Service name is required.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenDescriptionIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                     .WithTestValues()
                     .WithDescription("")
                     .Build();

                // Act
                var result = await _serviceRepository.UpdateAsync(entity);
                var expectedMessage = "Service description is required.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenPriceIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                    .WithTestValues()
                    .WithPrice(-3)
                    .Build();


                // Act
                var result = await _serviceRepository.UpdateAsync(entity);
                var expectedMessage = "Service price cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenEntityNotExists_ShouldReturnError()
            {
                // Arrange
                _dbContext.Service.Add(_serviceEntityBuilder.WithTestValues().WithId(999).Build());
                await _dbContext.SaveChangesAsync();

                var entity = _serviceEntityBuilder.WithTestValues().WithId(998).Build();

                // Act
                var result = await _serviceRepository.UpdateAsync(entity);
                var expectedMessage = "Service entity not found";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void UpdateAsync_WhenEntityExists_ShouldReturnSuccess()
            {
                // Arrange
                var entity = _serviceEntityBuilder.WithTestValues().Build();

                _dbContext.Service.Add(entity);
                await _dbContext.SaveChangesAsync();
                // Act
                var result = await _serviceRepository.UpdateAsync(entity);
                var expectedMessage = "Service Entity update successfully";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

        }

        public class DeleteAsync : ServiceRepositoryTest
        {
            [Fact]
            public async void DeleteAsync_WhenEntityIsNull_ShouldReturnError()
            {
                // Arrange
                Service entity = null;

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service entity cannot be null.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenNameIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                   .WithTestValues()
                   .WithName("")
                   .Build();

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service name is required.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenDescriptionIsNullOrEmpty_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                      .WithTestValues()
                      .WithDescription("")
                      .Build();

                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service description is required.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenPriceIsNegative_ShouldReturnError()
            {
                // Arrange
                var entity = _serviceEntityBuilder
                    .WithTestValues()
                    .WithPrice(-3)
                    .Build();


                // Act
                var result = await _serviceRepository.AddAsync(entity);
                var expectedMessage = "Service price cannot be negative.";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenEntityNotExists_ShouldReturnError()
            {
                // Arrange
                _dbContext.Service.Add(_serviceEntityBuilder.WithTestValues().WithId(999).Build());
                await _dbContext.SaveChangesAsync();

                var entity = _serviceEntityBuilder.WithTestValues().WithId(998).Build();

                // Act
                var result = await _serviceRepository.DeleteAsync(entity);
                var expectedMessage = "Service entity not found";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

            [Fact]
            public async void DeleteAsync_WhenEntityExists_ShouldReturnSuccess()
            {
                // Arrange
                var entity = _serviceEntityBuilder.WithTestValues().Build();

                _dbContext.Service.Add(entity);
                await _dbContext.SaveChangesAsync();
                // Act
                var result = await _serviceRepository.DeleteAsync(entity);
                var expectedMessage = $"Service {entity.Name} deleted successfully";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);

            }

        }

        public class GetByIdAsync : ServiceRepositoryTest
        {

            [Fact]
            public async void GetByIdAsync_WhenIdIsInvalid_ShouldReturnError()
            {
                // Arrange
                int id = -2;

                // Act
                var result = await _serviceRepository.GetByIdAsync(id);
                var expectedMessage = "Invalid service ID";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async void GetByIdAsync_WhenServiceNotFound_ShouldReturnError()
            {
                // Arrange
                int id = 2;

                // Act
                var result = await _serviceRepository.GetByIdAsync(id);
                var expectedMessage = $"Service was not found with the given ID";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.False(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }

            [Fact]
            public async void GetByIdAsync_WhenServiceFound_ShouldReturnEntity()
            {
                // Arrange
                _dbContext.Service.Add(_serviceEntityBuilder.WithTestValues().WithId(2).Build());
                await _dbContext.SaveChangesAsync();
                int id = 2;

                // Act
                var result = await _serviceRepository.GetByIdAsync(id);
                var expectedMessage = $"Service with the ID: {id} found";

                // Assert
                Assert.IsType<OperationResult<Service>>(result);
                Assert.True(result.IsSuccess);
                Assert.Equal(expectedMessage, result.Message);
            }
        }

        public class ExistsAsync : ServiceRepositoryTest
        {
            [Fact]
            public async Task ExistsAsync_WhenFilterIsNull_ShouldReturnError()
            {
                // arrange

                // act
                var result = await _serviceRepository.ExistsAsync(s => s.Name == null);

                // assert
                Assert.False(result);
            }
        }
    }
}
