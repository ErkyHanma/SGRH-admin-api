using SGRH.Application.Dtos.UserManagement.Client;
using Core.SGRH.Application.Interfaces.Repositories.UserManagement;
using Core.SGRH.Application.Interfaces.UserManagement;
using SGRH.Domain.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGRH.Application.Common.Logging;

namespace SGRH.Application.Services.UserManagement
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAppLogger<ClientService> _logger;

        public ClientService(IClientRepository clientRepository, IAppLogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<string> CreateClientAsync(CreateClientDto clientDto)
        {
            var newClient = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Phone = clientDto.Phone,
                Address = clientDto.Address,
                RoleId = clientDto.RoleId,
                PasswordHash = clientDto.PasswordHash,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1, // Placeholder
            };
            return await _clientRepository.CreateClientAsync(newClient);
        }

        public async Task<string> UpdateClientAsync(int id, UpdateClientDto clientDto)
        {
            var existingClient = await _clientRepository.GetClientByIdAsync(id);
            if (existingClient == null)
            {
                // CORRECCIÓN: Usando el método 'Info' de tu IAppLogger
                _logger.Info("UpdateClientAsync: Cliente con ID {0} no encontrado.", id);
                return "Error: Cliente no encontrado.";
            }

            existingClient.FirstName = clientDto.FirstName;
            existingClient.LastName = clientDto.LastName;
            existingClient.Email = clientDto.Email;
            existingClient.Phone = clientDto.Phone;
            existingClient.Address = clientDto.Address;
            existingClient.RoleId = clientDto.RoleId;
            existingClient.IsActive = clientDto.IsActive;
            existingClient.IsDeleted = clientDto.IsDeleted;
            existingClient.UpdatedAt = DateTime.UtcNow;
            existingClient.UpdatedBy = 1; // Placeholder

            if (!string.IsNullOrEmpty(clientDto.Password))
            {
                existingClient.PasswordHash = clientDto.Password;
            }

            return await _clientRepository.UpdateClientAsync(existingClient);
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _clientRepository.GetClientByIdAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _clientRepository.GetAllClientsAsync();
        }

        public async Task<string> DeleteClientAsync(int id)
        {
            return await _clientRepository.DeleteClientAsync(id);
        }
    }
}