using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SGRH.Web.Models;
using SGRH.Web.Models.Clients;
using SGRH.Web.ViewModels;
using System.Text.Json;
using System.Text;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SGRH.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        // Constructor que inyecta IConfiguration para obtener la URL base de la API.
        public ClientsController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ??
                          throw new InvalidOperationException("ApiSettings:BaseUrl not found in configuration.");

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);
        }

        // GET: /Clients/
        // Muestra una lista de todos los clientes.
        public async Task<IActionResult> Index()
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            try
            {
                // Realiza la petición GET a la API
                HttpResponseMessage response = await _httpClient.GetAsync("Clients");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    clients = JsonSerializer.Deserialize<List<ClientViewModel>>(apiResponse,
                                                                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                              ?? new List<ClientViewModel>();
                }
                else
                {
                    Debug.WriteLine($"ERROR: API returned a non-success status code: {response.StatusCode}");
                    ViewBag.ErrorMessage = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"ERROR: A network exception occurred while fetching clients: {ex.Message}");
                ViewBag.ErrorMessage = $"Network error: {ex.Message}";
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"ERROR: A JSON deserialization exception occurred: {ex.Message}");
                ViewBag.ErrorMessage = $"Data format error: {ex.Message}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: An unexpected exception occurred: {ex.Message}");
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }

            return View(clients);
        }

        // GET: ClientsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClientViewModel client = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Clients/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    client = JsonSerializer.Deserialize<ClientViewModel>(apiResponse,
                                                                          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (client == null)
                    {
                        ViewBag.ErrorMessage = $"Client with ID {id} not found or could not be deserialized.";
                        return View();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.ErrorMessage = $"Client with ID {id} not found.";
                    return View();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Error fetching client details: {response.StatusCode} - {errorContent}";
                    return View();
                }
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"Network error trying to fetch client details: {ex.Message}";
                return View();
            }
            catch (JsonException ex)
            {
                ViewBag.ErrorMessage = $"Data format error fetching client details: {ex.Message}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred while fetching client details: {ex.Message}";
                return View();
            }

            return View(client);
        }

        // GET: ClientsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateViewModel client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var jsonContent = JsonSerializer.Serialize(client);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync("Clients", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Cliente creado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        try
                        {
                            // Intenta deserializar el error de la API
                            var operationResult = JsonSerializer.Deserialize<OperationResult<string>>(errorResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            ViewBag.ErrorMessage = $"Error al crear el cliente: {operationResult?.Message ?? errorResponse}";
                        }
                        catch (JsonException)
                        {
                            ViewBag.ErrorMessage = $"Error al crear el cliente: {errorResponse}";
                        }
                        return View(client);
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = $"Error de red al intentar crear el cliente: {ex.Message}";
                }
                catch (JsonException ex)
                {
                    ViewBag.ErrorMessage = $"Error de formato de datos al crear el cliente: {ex.Message}";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Ocurrió un error inesperado al crear el cliente: {ex.Message}";
                }
            }
            return View(client);
        }

        // GET: ClientsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var response = await _httpClient.GetAsync($"Clients/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var clientString = await response.Content.ReadAsStringAsync();
                    var clientViewModel = JsonSerializer.Deserialize<ClientEditViewModel>(clientString,
                                                                                          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (clientViewModel == null)
                    {
                        return NotFound($"Could not deserialize client with ID {id}.");
                    }

                    return View(clientViewModel);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound($"Client with ID {id} not found.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Error fetching client: {response.StatusCode} - {errorContent}";
                    return View();
                }
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"Network error trying to fetch client: {ex.Message}";
                return View();
            }
            catch (JsonException ex)
            {
                ViewBag.ErrorMessage = $"Data format error fetching client: {ex.Message}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred while fetching client: {ex.Message}";
                return View();
            }
        }

        // POST: ClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientEditViewModel clientViewModel)
        {
            if (id != clientViewModel.UserId)
            {
                ViewBag.ErrorMessage = $"Error updating client: ID from route does not match client's UserId.";
                return View(clientViewModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var jsonContent = JsonSerializer.Serialize(clientViewModel);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"Clients/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Client updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        try
                        {
                            var operationResult = JsonSerializer.Deserialize<OperationResult<string>>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            ViewBag.ErrorMessage = $"Error updating client: {operationResult?.Message ?? errorContent}";
                        }
                        catch (JsonException)
                        {
                            ViewBag.ErrorMessage = $"Error updating client: {errorContent}";
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = $"Network error trying to update client: {ex.Message}";
                }
                catch (JsonException ex)
                {
                    ViewBag.ErrorMessage = $"Data format error updating client: {ex.Message}";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An unexpected error occurred while updating client: {ex.Message}";
                }
            }
            return View(clientViewModel);
        }

        // GET: ClientsController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClientViewModel client = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Clients/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    client = JsonSerializer.Deserialize<ClientViewModel>(apiResponse,
                                                                          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (client == null)
                    {
                        ViewBag.ErrorMessage = $"Client with ID {id} not found for deletion.";
                        return View();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.ErrorMessage = $"Client with ID {id} not found.";
                    return View();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Error fetching client for deletion: {response.StatusCode} - {errorContent}";
                    return View();
                }
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"Network error trying to fetch client for deletion: {ex.Message}";
                return View();
            }
            catch (JsonException ex)
            {
                ViewBag.ErrorMessage = $"Data format error fetching client for deletion: {ex.Message}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred while fetching client for deletion: {ex.Message}";
                return View();
            }

            return View(client);
        }

        // POST: ClientsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"Clients/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Client deleted successfully.";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    TempData["ErrorMessage"] = $"Client with ID {id} not found for deletion.";
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var operationResult = JsonSerializer.Deserialize<OperationResult<string>>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        TempData["ErrorMessage"] = $"Error deleting client: {operationResult?.Message ?? errorContent}";
                    }
                    catch (JsonException)
                    {
                        TempData["ErrorMessage"] = $"Error deleting client: {errorContent}";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = $"Network error trying to delete client: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting client: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
