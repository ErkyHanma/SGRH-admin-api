using SGRH.Application.Dtos.UserManagement.Client;
using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.SGRH.Application.Interfaces.UserManagement
{
    public interface IClientService
    {
        Task<string> CreateClientAsync(CreateClientDto clientDto);
        Task<string> UpdateClientAsync(int id, UpdateClientDto clientDto);
        Task<Client?> GetClientByIdAsync(int id);
        Task<string> DeleteClientAsync(int id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
    }
}