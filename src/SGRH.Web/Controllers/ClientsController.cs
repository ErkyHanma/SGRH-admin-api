// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Controllers\ClientsController.cs

using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Interfaces; // <-- Esta es la referencia correcta para el proyecto Web.
using SGRH.Web.Models;
using SGRH.Web.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
<<<<<<< Updated upstream
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Collections.Generic;
using SGRH.Web.Models.Clients;
using SGRH.Web.Models;
using System.Text;
=======

// IMPORTANTE: Asegúrate de que esta línea NO esté presente:
// using Core.SGRH.Application.Interfaces.UserManagement;
>>>>>>> Stashed changes

namespace SGRH.Web.Controllers
{
    public class ClientsController : Controller
    {
        // Dependency Injection of the Client Service.
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
<<<<<<< Updated upstream
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? throw new InvalidOperationException("ApiSettings:BaseUrl not found in configuration.");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);
        }

=======
            _clientService = clientService;
        }

        // GET: /Clients/
        // Displays a list of all clients.
>>>>>>> Stashed changes
        public async Task<IActionResult> Index()
        {
            try
            {
<<<<<<< Updated upstream
                HttpResponseMessage response = await _httpClient.GetAsync("Clients");
=======
                // Call the service to get the list of clients.
                var clients = await _clientService.GetClientsAsync();
>>>>>>> Stashed changes

                // If the clients list is null or empty, it logs a debug message
                // before returning the view. This is useful for debugging.
                if (clients == null || clients.Count == 0)
                {
<<<<<<< Updated upstream
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    clients = JsonSerializer.Deserialize<List<ClientViewModel>>(apiResponse,
                                                                 new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                                                 ?? new List<ClientViewModel>();
                }
                else
                {
                    ViewBag.ErrorMessage = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"Network error: {ex.Message}";
            }
            catch (JsonException ex)
            {
                ViewBag.ErrorMessage = $"Data format error: {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }

            return View(clients);
        }

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateViewModel client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var jsonContent = JsonSerializer.Serialize(client);
                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync("Clients", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Client created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        try
                        {
                            var operationResult = JsonSerializer.Deserialize<OperationResult<string>>(errorResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            ViewBag.ErrorMessage = $"Error creating client: {operationResult?.Message ?? errorResponse}";
                        }
                        catch (JsonException)
                        {
                            ViewBag.ErrorMessage = $"Error creating client: {errorResponse}";
                        }
                        return View(client);
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = $"Network error trying to create client: {ex.Message}";
                }
                catch (JsonException ex)
                {
                    ViewBag.ErrorMessage = $"Data format error creating client: {ex.Message}";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An unexpected error occurred while creating client: {ex.Message}";
                }
            }
            return View(client);
        }

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

                    clientViewModel.PasswordHash = null;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientEditViewModel clientViewModel)
        {
            ViewBag.DebugMessage = $"Route ID: {id}, ViewModel UserId: {clientViewModel.UserId}"; // Punto de interrupción aquí

            if (id != clientViewModel.UserId)
            {
                ViewBag.ErrorMessage = $"Error updating client: ID from route does not match client's UserId. {ViewBag.DebugMessage}";
                return View(clientViewModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var apiUpdateData = new
                    {
                        id = clientViewModel.UserId,
                        firstName = clientViewModel.FirstName,
                        lastName = clientViewModel.LastName,
                        email = clientViewModel.Email,
                        passwordHash = clientViewModel.PasswordHash,
                        roleId = clientViewModel.RoleId,
                        phone = clientViewModel.Phone,
                        address = clientViewModel.Address,
                        createdAt = clientViewModel.CreatedAt,
                        createdBy = clientViewModel.CreatedBy,
                        updatedAt = DateTime.UtcNow,
                        updatedBy = clientViewModel.UpdatedBy ?? 1,
                        deletedAt = clientViewModel.DeletedAt,
                        deletedBy = clientViewModel.DeletedBy,
                        isActive = clientViewModel.IsActive,
                        isDeleted = clientViewModel.IsDeleted
                    };

                    var jsonContent = JsonSerializer.Serialize(apiUpdateData);
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
                        ViewBag.ErrorMessage = $"Client with ID {id} not found or could not be deserialized for deletion.";
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
                    return RedirectToAction(nameof(Index));
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
=======
                    Debug.WriteLine("DEBUG: The clients list is empty or null.");
                }

                // Pass the list to the view.
                return View(clients);
            }
            catch (Exception ex)
            {
                // Log the exception for detailed error information.
                Debug.WriteLine($"ERROR: An exception occurred while fetching clients: {ex.Message}");
                // Return an error view or a redirect to a general error page.
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
>>>>>>> Stashed changes
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting client: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
