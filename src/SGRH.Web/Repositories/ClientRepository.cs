// Archivo: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Repositories\ClientRepository.cs
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SGRH.Web.Models; // Nos aseguramos de que el namespace del modelo sea correcto

namespace SGRH.Web.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly HttpClient _httpClient;

        public ClientRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtiene una lista de todos los clientes de la API.
        /// </summary>
        /// <returns>Una lista de objetos Client o null si ocurre un error.</returns>
        public async Task<List<Client>?> GetAllClientsAsync()
        {
            try
            {
                // Realiza la solicitud GET a la API
                var response = await _httpClient.GetAsync("api/clients");
                // Lanza una excepción si la respuesta no es exitosa
                response.EnsureSuccessStatusCode();
                // Lee el contenido de la respuesta como una cadena
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserializa la cadena JSON en una lista de objetos Client
                return JsonSerializer.Deserialize<List<Client>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException e)
            {
                // Maneja los errores de la solicitud HTTP
                Console.WriteLine($"Error al obtener clientes: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene un cliente por su ID de la API.
        /// </summary>
        /// <param name="id">El ID del cliente.</param>
        /// <returns>El objeto Client o null si no se encuentra o si ocurre un error.</returns>
        public async Task<Client?> GetClientByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/clients/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Client>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al obtener el cliente con ID {id}: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Crea un nuevo cliente enviando los datos a la API.
        /// </summary>
        /// <param name="client">El objeto Client a crear.</param>
        /// <returns>True si el cliente fue creado exitosamente, de lo contrario False.</returns>
        public async Task<bool> CreateClientAsync(Client client)
        {
            try
            {
                // Serializa el objeto client a JSON y lo empaqueta en StringContent
                var jsonContent = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");
                // Realiza la solicitud POST a la API
                var response = await _httpClient.PostAsync("api/clients", jsonContent);
                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al crear el cliente: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Actualiza un cliente existente en la API.
        /// </summary>
        /// <param name="client">El objeto Client con los datos actualizados.</param>
        /// <returns>True si el cliente fue actualizado exitosamente, de lo contrario False.</returns>
        public async Task<bool> UpdateClientAsync(Client client)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/clients/{client.Id}", jsonContent);
                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al actualizar el cliente: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina un cliente de la API por su ID.
        /// </summary>
        /// <param name="id">El ID del cliente a eliminar.</param>
        /// <returns>True si el cliente fue eliminado exitosamente, de lo contrario False.</returns>
        public async Task<bool> DeleteClientAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/clients/{id}");
                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al eliminar el cliente: {e.Message}");
                return false;
            }
        }
    }
}
