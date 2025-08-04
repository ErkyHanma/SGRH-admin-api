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

namespace SGRH.Web.Services
{
    // Implementation of the IClientService for the web application.
    // This service handles all API calls related to clients.
    public class ClientService : IClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5171/api/clients"; // Correct API URL

        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Retrieves all clients from the API and deserializes them.
        public async Task<List<ClientViewModel>> GetClientsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return clients;
            }
            catch (Exception)
            {
                // In a real app, you would log the exception here.
                return new List<ClientViewModel>();
            }
        }

        // Creates a new client by sending a POST request to the API.
        public async Task CreateClientAsync(ClientCreateViewModel clientViewModel)
        {
            // The password hashing logic will go here.
            var jsonContent = JsonSerializer.Serialize(clientViewModel);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiBaseUrl, httpContent);
            response.EnsureSuccessStatusCode();
        }
    }
}
