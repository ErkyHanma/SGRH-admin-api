// Archivo: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Repositories\IClientRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using SGRH.Web.Models; // Asegúrate de que el namespace del modelo sea correcto

namespace SGRH.Web.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>?> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<bool> CreateClientAsync(Client client);
        Task<bool> UpdateClientAsync(Client client);
        Task<bool> DeleteClientAsync(int id);
    }
}
