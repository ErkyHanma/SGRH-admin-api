// File: SGRH.Web/Interfaces/IClientService.cs

using SGRH.Web.Models.Clients;
using SGRH.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Web.Interfaces
{
    // Asegúrate de que los métodos de la interfaz coincidan exactamente con
    // la implementación en ClientService.cs.
    public interface IClientService
    {
        Task<List<ClientViewModel>> GetClientsAsync();
        Task<ClientViewModel> GetClientByIdAsync(int id);
        Task<bool> CreateClientAsync(ClientCreateViewModel clientViewModel);
        Task<bool> UpdateClientAsync(int id, ClientEditViewModel clientViewModel);
        Task<bool> DeleteClientAsync(int id);
    }
}
