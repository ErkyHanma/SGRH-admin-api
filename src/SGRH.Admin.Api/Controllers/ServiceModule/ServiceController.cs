using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.ServiceModule;
using SGRH.Application.Interfaces.Services.Service_Module;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGRH.Api.Controllers.ServiceModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }



        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var result = await _serviceService.GetAllServicesAsync();

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _serviceService.GetServiceByIdAsync(id);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // POST api/<ReservationController>
        [HttpPost("CreateService")]
        public async Task<IActionResult> Post([FromBody] CreateServiceDto createServiceDto)
        {
            var result = await _serviceService.CreateServicesAsync(createServiceDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/<ReservationController>/5
        [HttpPost("UpdateService")]
        public async Task<IActionResult> Post([FromBody] ServiceDto serviceDto)
        {
            var result = await _serviceService.UpdateServicesAsync(serviceDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("DisableService")]
        public async Task<IActionResult> DeleteService([FromBody] DeleteServiceDto deleteServiceDto)
        {
            var result = await _serviceService.DeleteServicesAsync(deleteServiceDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
