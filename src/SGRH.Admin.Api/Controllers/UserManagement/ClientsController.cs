using Core.SGRH.Application.Interfaces.Repositories.UserManagement;
using Core.SGRH.Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Mvc;
<<<<<<< Updated upstream
using SGRH.Domain.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
=======
using SGRH.Application.Dtos.UserManagement.Client;
using System.Linq;
using System.Threading.Tasks;
>>>>>>> Stashed changes

namespace SGRH.Api.Controllers.UserManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
<<<<<<< Updated upstream
            var clients = await _clientRepository.GetAllClientsAsync();

=======
            var clients = await _clientService.GetAllClientsAsync();
>>>>>>> Stashed changes
            if (clients == null || !clients.Any())
            {
                return NotFound("No clients found.");
            }
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
<<<<<<< Updated upstream
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost] //Corregido el 31/07/2025 porque se buscaba un mensaje en español y daba un error diferente al crear un cliente, generaba problemas para el consumo de API.
        public async Task<IActionResult> Create([FromBody] Client client)
=======
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound($"Cliente con ID {id} no encontrado.");
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto clientDto)
>>>>>>> Stashed changes
        {
            var result = await _clientService.CreateClientAsync(clientDto);

<<<<<<< Updated upstream
            if (result.Contains("successfully"))
            {
                return CreatedAtAction(nameof(GetById), new { id = client.UserId }, client);
=======
            if (result.Contains("éxito") || result.Contains("creado"))
            {
                // Como no tenemos el ID del cliente creado, devolvemos Ok en lugar de CreatedAtAction.
                // Es una buena práctica modificar el servicio para que devuelva el objeto creado completo.
                return Ok(result);
>>>>>>> Stashed changes
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDto clientDto)
        {
<<<<<<< Updated upstream
            if (id != client.UserId)
            {
                return BadRequest("Route ID does not match client's UserId.");
            }

            var result = await _clientRepository.UpdateClientAsync(client);
            if (result.Contains("success") || result.Contains("updated"))
            {
                return NoContent();
            }
=======
            // Verificamos si el ID de la ruta coincide con el ID del DTO
            if (id != clientDto.UserId)
            {
                return BadRequest("El ID de la ruta no coincide con el UserId del cliente en el cuerpo de la petición.");
            }

            var result = await _clientService.UpdateClientAsync(id, clientDto);

            if (result.Contains("éxito") || result.Contains("actualizado"))
            {
                return NoContent();
            }
>>>>>>> Stashed changes
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
<<<<<<< Updated upstream
            int updatedByUserId = 1;

            var result = await _clientRepository.DisableClientAsync(id, updatedByUserId);
            if (result.Contains("success") || result.Contains("disabled"))
            {
                return NoContent();
            }
            return BadRequest(result);
=======
            var result = await _clientService.DeleteClientAsync(id);

            if (result.Contains("éxito") || result.Contains("eliminado") || result.Contains("deleted"))
            {
                return NoContent(); // 204 No Content para indicar que la operación fue exitosa
            }
            return NotFound(result); // Devolvemos NotFound si el cliente no existe
>>>>>>> Stashed changes
        }
    }
}