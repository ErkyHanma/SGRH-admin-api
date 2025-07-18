
using SGRH.Domain.Entities.UserManagement; 
using System.Threading.Tasks;



namespace SGRH.Application.Interfaces.UserManagement
{
    public interface IClientService
    {
        Task<string> CreateClientAsync(Client client);
        Task<Client> GetClientByIdAsync(int clientId);
        Task<string> UpdateClientAsync(Client client);
        Task<string> DisableClientAsync(int clientId);
    }
}