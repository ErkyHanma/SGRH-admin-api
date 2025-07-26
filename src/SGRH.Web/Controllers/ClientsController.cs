using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json; // used for JSON serialization/deserialization
using System.Collections.Generic; // used for lists (e.g., list of clients)
using SGRH.Web.Models.Clients; // used for ClientViewModel, ClientCreateViewModel, ClientEditViewModel
using SGRH.Web.Models; // Used for OperationResult
using Microsoft.AspNetCore.Http; // Needed for IFormCollection in POST actions

namespace SGRH.Web.Controllers
{
    public class ClientsController : Controller // Class name is now ClientsController (plural)
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ClientsController(IConfiguration configuration)
        {
            // Read API base URL from appsettings.json
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? throw new InvalidOperationException("ApiSettings:BaseUrl not found in configuration.");

            // Initialize HttpClient and set its base address
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);
        }

        // GET: ClientsController
        public async Task<IActionResult> Index()
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            try
            {
                // Making the GET request to the API
                // Assuming your API endpoint for getting all clients is /api/Clients
                HttpResponseMessage response = await _httpClient.GetAsync("Clients");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the API response directly into a List<ClientViewModel>
                    clients = JsonSerializer.Deserialize<List<ClientViewModel>>(apiResponse,
                                                                 new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                                                 ?? new List<ClientViewModel>(); // Handle null result
                }
                else
                {
                    // Handle non-successful HTTP status codes
                    ViewBag.ErrorMessage = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network or request-specific errors
                ViewBag.ErrorMessage = $"Network error: {ex.Message}";
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization errors
                ViewBag.ErrorMessage = $"Data format error: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }

            return View(clients); // Pass the list of clients to the view
        }

        // GET: ClientsController/Details/5
        public IActionResult Details(int id)
        {
            // This action will later fetch details for a specific client from the API.
            return View();
        }

        // GET: ClientsController/Create
        public IActionResult Create()
        {
            // This action will simply display the form to create a new client.
            return View();
        }

        // POST: ClientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateViewModel client) // Este es el cambio que estábamos implementando
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
                        TempData["SuccessMessage"] = "Cliente creado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        try
                        {
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
        public IActionResult Edit(int id)
        {
            // This action will later fetch client data for the edit form.
            return View();
        }

        // POST: ClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection) // Placeholder, will be ClientEditViewModel later
        {
            try
            {
                // Placeholder for API call logic
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Placeholder for error handling
                return View();
            }
        }

        // GET: ClientsController/Delete/5
        public IActionResult Delete(int id)
        {
            // This action will later fetch client data for delete confirmation.
            return View();
        }

        // POST: ClientsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")] // Needed to differentiate from GET Delete when both are named "Delete" by convention
        public IActionResult DeleteConfirmed(int id) // Placeholder for ViewModel/collection
        {
            try
            {
                // Placeholder for API call logic
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Placeholder for error handling
                return View();
            }
        }
    }
}