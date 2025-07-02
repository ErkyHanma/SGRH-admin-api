using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//using SGRH.Application.Common.Logging; // Mantener si se usará el logger en el futuro, aunque no esté en el constructor actual
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Services.Hotel;

namespace SGRH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomCategoryController : ControllerBase
    {
        private readonly IRoomCategoryService _roomCategoryService;
        // private readonly IAppLogger<RoomCategoryController> _logger; // Comentado por consistencia con RoomController
        // private readonly IConfiguration _configuration; // Comentado por consistencia con RoomController

        public RoomCategoryController(IRoomCategoryService roomCategoryService)
        {
            _roomCategoryService = roomCategoryService;
        }

        // GET: api/RoomCategory
        [HttpGet("GetRoomCategories")]
        public async Task<IActionResult> Get()
        {
            var result = await _roomCategoryService.GetRoomCategories();
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET api/RoomCategory/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _roomCategoryService.GetRoomCategoryById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // POST api/RoomCategory/CreateRoomCategory
        [HttpPost("CreateRoomCategory")]
        public async Task<IActionResult> Post([FromBody] CreateRoomCategoryDto createRoomCategoryDto)
        {
            var result = await _roomCategoryService.CreateRoomCategory(createRoomCategoryDto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/RoomCategory/ModifyRoomCategory
        [HttpPut("ModifyRoomCategory")]
        public async Task<IActionResult> ModifyRoomCategory([FromBody] ModifyRoomCategoryDto modifyRoomCategoryDto)
        {
            var result = await _roomCategoryService.UpdateRoomCategory(modifyRoomCategoryDto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/RoomCategory/DisableRoomCategory
        [HttpPut("DisableRoomCategory")] // Usamos PUT por convención para deshabilitar (actualizar estado)
        public async Task<IActionResult> DisableRoomCategory([FromBody] DisableRoomCategoryDto disableRoomCategoryDto)
        {
            var result = await _roomCategoryService.DeleteRoomCategory(disableRoomCategoryDto); // El servicio llama a DeleteAsync
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}