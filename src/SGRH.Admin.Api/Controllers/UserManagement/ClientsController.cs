using Core.Application.Interfaces.Repositories.UserManagement;
using Microsoft.AspNetCore.Mvc;
using SGRH.Domain.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGRH.Api.Controllers.UserManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientRepository.GetAllClientsAsync();

            if (clients == null || !clients.Any())
            {
                return NotFound("No clients found.");
            }
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost] //Corregido el 31/07/2025 porque se buscaba un mensaje en español y daba un error diferente al crear un cliente, generaba problemas para el consumo de API.
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            var result = await _clientRepository.CreateClientAsync(client);

            if (result.Contains("successfully"))
            {
                return CreatedAtAction(nameof(GetById), new { id = client.UserId }, client);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            if (id != client.UserId)
            {
                return BadRequest("Route ID does not match client's UserId.");
            }

            var result = await _clientRepository.UpdateClientAsync(client);
            if (result.Contains("success") || result.Contains("updated"))
            {
                return NoContent();
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int updatedByUserId = 1;

            var result = await _clientRepository.DisableClientAsync(id, updatedByUserId);
            if (result.Contains("success") || result.Contains("disabled"))
            {
                return NoContent();
            }
            return BadRequest(result);
        }
    }
}