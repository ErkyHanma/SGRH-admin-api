// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Controllers\ClientsController.cs

using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Interfaces;
using SGRH.Web.Models.Clients;
using SGRH.Web.ViewModels;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;

namespace SGRH.Web.Controllers
{
    public class ClientsController : Controller
    {
        // Usamos la interfaz del servicio, no un HttpClient.
        private readonly IClientService _clientService;

        // Inyectamos el IClientService, que ya tiene el HttpClient.
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: /Clients/
        public async Task<IActionResult> Index()
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            try
            {
                // Delegamos la llamada al servicio.
                clients = await _clientService.GetClientsAsync();
            }
            catch (Exception ex)
            {
                // La gestión de errores se puede simplificar aquí, ya que el servicio
                // maneja la mayoría de los casos.
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
                // Delegamos la llamada al servicio.
                client = await _clientService.GetClientByIdAsync(id.Value);

                if (client == null)
                {
                    ViewBag.ErrorMessage = $"Client with ID {id} not found.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: An unexpected exception occurred: {ex.Message}");
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
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
                    // Delegamos la llamada al servicio.
                    bool success = await _clientService.CreateClientAsync(client);

                    if (success)
                    {
                        TempData["SuccessMessage"] = "Cliente creado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al crear el cliente. Por favor, revisa el formato de los datos.";
                    }
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

            ClientViewModel clientViewModel = null;
            try
            {
                // Delegamos la llamada al servicio.
                clientViewModel = await _clientService.GetClientByIdAsync(id.Value);

                if (clientViewModel == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred while fetching client for editing: {ex.Message}";
                return View();
            }

            // Mapeamos el ClientViewModel a ClientEditViewModel para la vista
            var editViewModel = new ClientEditViewModel
            {
                UserId = clientViewModel.Id,
                FirstName = clientViewModel.FirstName,
                LastName = clientViewModel.LastName,
                Email = clientViewModel.Email,
                Phone = clientViewModel.Phone,
                Address = clientViewModel.Address,
                RoleId = clientViewModel.RoleId,
                IsActive = clientViewModel.IsActive,
                // Conservamos los datos de auditoría
                CreatedAt = clientViewModel.CreatedAt,
                CreatedBy = clientViewModel.CreatedBy,
                UpdatedAt = clientViewModel.UpdatedAt,
                UpdatedBy = clientViewModel.UpdatedBy,
                IsDeleted = clientViewModel.IsDeleted,
                // No incluimos la contraseña ni el hash
            };

            return View(editViewModel);
        }


        // POST: ClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientEditViewModel clientViewModel)
        {
            if (id != clientViewModel.UserId)
            {
                ViewBag.ErrorMessage = $"Error: ID from route does not match client's UserId.";
                return View(clientViewModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Delegamos la llamada al servicio.
                    bool success = await _clientService.UpdateClientAsync(id, clientViewModel);

                    if (success)
                    {
                        TempData["SuccessMessage"] = "Client updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error updating client. Please check the data format.";
                    }
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
                // Delegamos la llamada al servicio.
                client = await _clientService.GetClientByIdAsync(id.Value);

                if (client == null)
                {
                    ViewBag.ErrorMessage = $"Client with ID {id} not found for deletion.";
                    return View();
                }
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
                // Delegamos la llamada al servicio.
                bool success = await _clientService.DeleteClientAsync(id);

                if (success)
                {
                    TempData["SuccessMessage"] = "Client deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting client.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting client: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
