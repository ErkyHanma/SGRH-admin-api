using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Common.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//using SGRH.Application.Common.Logging; // Mantener si se usará el logger en el futuro, aunque no esté en el constructor actual
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Services.Hotel;

namespace SGRH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorService;
        private readonly IAppLogger<FloorController> _logger;
        private readonly IConfiguration _configuration;     

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        // GET: api/Floor
        [HttpGet("GetFloors")]
        public async Task<IActionResult> Get()
        {
            var result = await _floorService.GetFloors();
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET api/Floor/5
        [HttpGet("{GetFloorById}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _floorService.GetFloorsById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // POST api/Floor/CreateFloor
        [HttpPost("CreateFloor")]
        public async Task<IActionResult> Post([FromBody] CreateFloorDto createFloorDto)
        {
            var result = await _floorService.CreateFloor(createFloorDto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/Floor/ModifyFloor
        [HttpPut("ModifyFloor")]
        public async Task<IActionResult> ModifyFloor([FromBody] ModifyFloorDto modifyFloorDto)
        {
            var result = await _floorService.UpdateFloor(modifyFloorDto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/Floor/DisableFloor
        [HttpPut("DisableFloor")] // Usamos PUT por convención para deshabilitar (actualizar estado)
        public async Task<IActionResult> DisableFloor([FromBody] DisableFloorDto disableFloorDto)
        {
            var result = await _floorService.DeleteFloor(disableFloorDto); // El servicio llama a DeleteAsync
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}