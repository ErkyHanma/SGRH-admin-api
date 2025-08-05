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
    public class ClientService : IClientService
    {
        private readonly HttpClient _httpClient;

        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ClientViewModel>> GetClientsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Clients");
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
                var response = await _httpClient.GetAsync($"Clients/{id}");
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
                var jsonContent = JsonSerializer.Serialize(clientViewModel);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Clients", httpContent);
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
                var jsonContent = JsonSerializer.Serialize(clientViewModel);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // CORRECCIÓN: Manejamos explícitamente el código de estado.
                var response = await _httpClient.PutAsync($"Clients/{id}", httpContent);

                // Si el estado de la respuesta es un éxito, el método devuelve true.
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Si no es un éxito, lee el contenido de la respuesta para el mensaje de error.
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error al actualizar cliente. Código de estado: {response.StatusCode}. Contenido: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error inesperado al actualizar cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Clients/{id}");
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
