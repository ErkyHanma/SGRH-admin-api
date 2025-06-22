using SGRH.Domain.Entities.UserManagement;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repositories.UserManagement
{
    public interface IClientRepository
    {
        Task<string> CreateClientAsync(Client client);
        Task<Client?> GetClientByIdAsync(int clientId);
        Task<string> UpdateClientAsync(Client client);
        Task<string> DisableClientAsync(int clientId, int updatedBy);
    }
}