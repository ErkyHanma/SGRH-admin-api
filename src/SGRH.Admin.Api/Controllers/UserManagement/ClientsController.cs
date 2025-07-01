using Core.Application.Interfaces.Repositories.UserManagement;
using Microsoft.AspNetCore.Mvc;
using SGRH.Domain.Entities.UserManagement;
using System; 
using System.Collections.Generic; 

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
           
            throw new NotImplementedException("El método GetAllAsync no está definido en IClientRepository o su contraparte.");

        }
              
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id); 
            if (client == null) return NotFound();
            return Ok(client);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            var result = await _clientRepository.CreateClientAsync(client);

            if (result.Contains("éxito") || result.Contains("creado")) 
            {
                
                return CreatedAtAction(nameof(GetById), new { id = client.UserId }, client);
            }
            return BadRequest(result); 
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            
            if (id != client.UserId) return BadRequest("ID de la ruta no coincide con el UserId del cliente.");

            var result = await _clientRepository.UpdateClientAsync(client);
            if (result.Contains("éxito") || result.Contains("actualizado")) 
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
            if (result.Contains("éxito") || result.Contains("deshabilitado")) 
            {
                return NoContent(); // 204 No Content
            }
            return BadRequest(result); 
        }
    }
}
