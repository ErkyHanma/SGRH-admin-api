using Core.Application.Interfaces.Repositories.UserManagement;
using SGRH.Application.Common.Logging; // Necessary for the logger (IAppLogger)
using SGRH.Application.Interfaces.UserManagement; // Required for IClientService inheritance
using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic; // For IEnumerable<T>
using System.Threading.Tasks; // For Task



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

       
        public async Task<string> CreateClientAsync(Client client)
        {
            _logger.Info($"Creating client: {client.FirstName} {client.LastName}.");
            
            var result = await _clientRepository.CreateClientAsync(client);
            return result;
        }

        public async Task<Client> GetClientByIdAsync(int clientId)
        {
            _logger.Info($"Getting client with Id: {clientId}.");
            
            var result = await _clientRepository.GetClientByIdAsync(clientId);
            return result;
        }

        public async Task<string> UpdateClientAsync(Client client)
        {
            _logger.Info($"Updating client with Id: {client.UserId}.");
            
            var result = await _clientRepository.UpdateClientAsync(client);
            return result;
        }

        public async Task<string> DisableClientAsync(int clientId)
        {
            _logger.Info($"Disabling client with Id: {clientId}.");
            var result = await _clientRepository.DisableClientAsync(clientId, 1); 
            return result;
        }

        
    }
}