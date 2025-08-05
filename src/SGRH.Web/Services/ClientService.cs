// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Services\ClientService.cs

using SGRH.Web.Interfaces;
using SGRH.Web.Models.Clients;
using SGRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SGRH.Web.Services
{
    // El servicio ahora se centra exclusivamente en la comunicación con la API.
    public class ClientService : IClientService
    {
        private readonly HttpClient _httpClient;

        // El constructor ahora solo inyecta el HttpClient.
        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ClientViewModel>> GetClientsAsync()
        {
            try
            {
                // Agregamos el prefijo 'api/' a la URL para que coincida con la API.
                var response = await _httpClient.GetAsync("api/Clients");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(content,
                                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return clients ?? new List<ClientViewModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener clientes: {ex.Message}");
                return new List<ClientViewModel>();
            }
        }

        public async Task<ClientViewModel> GetClientByIdAsync(int id)
        {
            try
            {
                // Agregamos el prefijo 'api/' a la URL para que coincida con la API.
                var response = await _httpClient.GetAsync($"api/Clients/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClientViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener cliente por ID: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateClientAsync(ClientCreateViewModel clientViewModel)
        {
            try
            {
                // Usamos el `using SGRH.Web.ViewModels;` para evitar el error de tipo.
                var jsonContent = JsonSerializer.Serialize(clientViewModel);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Agregamos el prefijo 'api/' a la URL para que coincida con la API.
                var response = await _httpClient.PostAsync("api/Clients", httpContent);

                // Si la creación fue exitosa, devolvemos true.
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al crear cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateClientAsync(int id, ClientEditViewModel clientViewModel)
        {
            try
            {
                // Usamos el `using SGRH.Web.Models.Clients;` para evitar el error de tipo.
                var jsonContent = JsonSerializer.Serialize(clientViewModel);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Agregamos el prefijo 'api/' a la URL para que coincida con la API.
                var response = await _httpClient.PutAsync($"api/Clients/{id}", httpContent);

                // Si la actualización fue exitosa, devolvemos true.
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            try
            {
                // Agregamos el prefijo 'api/' a la URL para que coincida con la API.
                var response = await _httpClient.DeleteAsync($"api/Clients/{id}");

                // Si la eliminación fue exitosa, devolvemos true.
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar cliente: {ex.Message}");
                return false;
            }
        }
    }
}
