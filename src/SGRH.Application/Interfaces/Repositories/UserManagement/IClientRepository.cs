using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.SGRH.Application.Interfaces.Repositories.UserManagement
{
    public interface IClientRepository
    {
        Task<string> CreateClientAsync(Client client);
        Task<string> UpdateClientAsync(Client client);
        Task<Client?> GetClientByIdAsync(int id);
        Task<string> DeleteClientAsync(int id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
    }
}