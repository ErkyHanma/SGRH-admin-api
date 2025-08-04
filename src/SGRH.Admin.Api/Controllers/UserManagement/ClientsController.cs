using Core.SGRH.Application.Interfaces.Repositories.UserManagement;
using Core.SGRH.Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.UserManagement.Client;
using System.Linq;
using System.Threading.Tasks;

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
            var clients = await _clientService.GetAllClientsAsync();
            if (clients == null || !clients.Any())
            {
                return NotFound("No clients found.");
            }
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound($"Cliente con ID {id} no encontrado.");
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto clientDto)
        {
            var result = await _clientService.CreateClientAsync(clientDto);

            if (result.Contains("éxito") || result.Contains("creado"))
            {
                // Como no tenemos el ID del cliente creado, devolvemos Ok en lugar de CreatedAtAction.
                // Es una buena práctica modificar el servicio para que devuelva el objeto creado completo.
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDto clientDto)
        {
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
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clientService.DeleteClientAsync(id);

            if (result.Contains("éxito") || result.Contains("eliminado") || result.Contains("deleted"))
            {
                return NoContent(); // 204 No Content para indicar que la operación fue exitosa
            }
            return BadRequest(result); 
        }
    }
}