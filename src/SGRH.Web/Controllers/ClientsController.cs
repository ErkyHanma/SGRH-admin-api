using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Collections.Generic;
using SGRH.Web.Models.Clients;
using SGRH.Web.Models;
using System.Text;

namespace SGRH.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ClientsController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? throw new InvalidOperationException("ApiSettings:BaseUrl not found in configuration.");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);
        }

        public async Task<IActionResult> Index()
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            try
            {
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
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting client: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
