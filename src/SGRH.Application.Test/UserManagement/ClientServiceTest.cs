using Core.Application.Interfaces.Repositories.UserManagement;
using Moq;
using SGRH.Application.Common.Logging;
using SGRH.Application.Interfaces.UserManagement;
using SGRH.Application.Services.UserManagement;
using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


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
            var client = new Client { FirstName = "Test", LastName = "User", Email = "test@correo.com" };
            _clientRepositoryMock
                .Setup(repo => repo.CreateClientAsync(client))
                .ReturnsAsync("Cliente creado con éxito");

            // Act
            var result = await _clientService.CreateClientAsync(client);

            // Assert
            Assert.Equal("Cliente creado con éxito", result);
            _clientRepositoryMock.Verify(repo => repo.CreateClientAsync(client), Times.Once);
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
            var client = new Client { UserId = 1, FirstName = "Modificado" };
            _clientRepositoryMock
                .Setup(repo => repo.UpdateClientAsync(client))
                .ReturnsAsync("Cliente actualizado correctamente");

            // Act
            var result = await _clientService.UpdateClientAsync(client);

            // Assert
            Assert.Equal("Cliente actualizado correctamente", result);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientAsync(client), Times.Once);
        }

        [Fact]
        public async Task DisableClientAsync_ShouldReturnSuccessMessage()
        {
            // Arrange
            int clientId = 1;
            _clientRepositoryMock
                .Setup(repo => repo.DisableClientAsync(clientId, It.IsAny<int>()))
                .ReturnsAsync("Cliente desactivado correctamente");

            // Act
            var result = await _clientService.DisableClientAsync(clientId);

            // Assert
            Assert.Equal("Cliente desactivado correctamente", result);
            _clientRepositoryMock.Verify(repo => repo.DisableClientAsync(clientId, It.IsAny<int>()), Times.Once);
        }
    }
}