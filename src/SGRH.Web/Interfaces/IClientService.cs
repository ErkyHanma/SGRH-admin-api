using SGRH.Web.Models.Clients;
using SGRH.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Web.Interfaces
{
    // Interface for the client service in the web application.
    // It defines the methods to interact with the API.
    public interface IClientService
    {
        // Asynchronously retrieves a list of all clients from the API.
        Task<List<ClientViewModel>> GetClientsAsync();

        // Asynchronously creates a new client via the API.
        Task CreateClientAsync(ClientCreateViewModel clientViewModel);
    }
}
