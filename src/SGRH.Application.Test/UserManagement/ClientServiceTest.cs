using Core.SGRH.Application.Interfaces.Repositories.UserManagement;
using Moq;
using SGRH.Application.Common.Logging;
using Core.SGRH.Application.Interfaces.UserManagement;
using SGRH.Application.Services.UserManagement;
using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using SGRH.Application.Dtos.UserManagement.Client; // Necesario para usar los DTOs

namespace SGRH.Application.Test.UserManagement
{
    public class ClientServiceTest
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IAppLogger<ClientService>> _loggerMock;
        private readonly ClientService _clientService;

        public ClientServiceTest()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _loggerMock = new Mock<IAppLogger<ClientService>>();
            _clientService = new ClientService(_clientRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateClientAsync_ShouldReturnSuccessMessage()
        {
            // Arrange
            // El servicio espera un CreateClientDto. Lo creamos.
            var createClientDto = new CreateClientDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@correo.com",
                Phone = "1234567890"
            };

            // El servicio convierte el DTO en una entidad Client y la pasa al repositorio.
            // Por lo tanto, el mock del repositorio debe esperar un objeto Client.
            _clientRepositoryMock
                .Setup(repo => repo.CreateClientAsync(It.IsAny<Client>()))
                .ReturnsAsync("Cliente creado con éxito");

            // Act
            // Llamamos al método del servicio con el DTO.
            var result = await _clientService.CreateClientAsync(createClientDto);

            // Assert
            Assert.Equal("Cliente creado con éxito", result);
            _clientRepositoryMock.Verify(repo => repo.CreateClientAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task GetClientByIdAsync_ShouldReturnClient()
        {
            // Arrange
            var client = new Client { UserId = 1, FirstName = "Juan" };
            _clientRepositoryMock
                .Setup(repo => repo.GetClientByIdAsync(1))
                .ReturnsAsync(client);

            // Act
            var result = await _clientService.GetClientByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Juan", result.FirstName);
            _clientRepositoryMock.Verify(repo => repo.GetClientByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdateClientAsync_ShouldReturnSuccessMessage()
        {
            // Arrange
            // El servicio espera un ID y un UpdateClientDto.
            var clientId = 1;
            var updateClientDto = new UpdateClientDto
            {
                FirstName = "Modificado",
                Email = "modificado@correo.com"
            };

            // Para que el servicio pueda actualizar el cliente, necesitamos simular
            // que GetClientByIdAsync devuelve un cliente existente.
            _clientRepositoryMock
                .Setup(repo => repo.GetClientByIdAsync(clientId))
                .ReturnsAsync(new Client { UserId = clientId });

            // El servicio recibe el DTO, lo usa para actualizar el cliente existente y luego
            // lo pasa como entidad al repositorio. El mock del repositorio debe esperar un objeto Client.
            _clientRepositoryMock
                .Setup(repo => repo.UpdateClientAsync(It.IsAny<Client>()))
                .ReturnsAsync("Cliente actualizado correctamente");

            // Act
            // Llamamos al método del servicio con el ID y el DTO.
            var result = await _clientService.UpdateClientAsync(clientId, updateClientDto);

            // Assert
            Assert.Equal("Cliente actualizado correctamente", result);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task DeleteClientAsync_ShouldReturnSuccessMessage()
        {
            // Arrange
            int clientId = 1;
            _clientRepositoryMock
                .Setup(repo => repo.DeleteClientAsync(clientId))
                .ReturnsAsync("Cliente eliminado correctamente");

            // Act
            var result = await _clientService.DeleteClientAsync(clientId);

            // Assert
            Assert.Equal("Cliente eliminado correctamente", result);
            _clientRepositoryMock.Verify(repo => repo.DeleteClientAsync(clientId), Times.Once);
        }

        [Fact]
        public async Task GetAllClientsAsync_ShouldReturnListOfClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { UserId = 1, FirstName = "Cliente1" },
                new Client { UserId = 2, FirstName = "Cliente2" }
            };
            _clientRepositoryMock.Setup(repo => repo.GetAllClientsAsync()).ReturnsAsync(clients);

            // Act
            var result = await _clientService.GetAllClientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Client>)result).Count);
            _clientRepositoryMock.Verify(repo => repo.GetAllClientsAsync(), Times.Once);
        }
    }
}
