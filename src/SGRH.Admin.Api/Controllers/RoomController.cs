using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Common.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGRH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IAppLogger<RoomController> _logger;
        private readonly IConfiguration _configuration;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;

        }

        // GET: api/<RoomController>
        [HttpGet("GetRooms")]
        public async Task<IActionResult> Get()
        {
            var result = await _roomService.GetRooms();
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);

        }

        // GET api/<RoomController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _roomService.GetRoomsById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // POST api/<RoomController>
        [HttpPost("CreateRoom")]
        public async Task<IActionResult> Post([FromBody] CreateRoomDto createRoomDto)
        {
            var result = await _roomService.CreateRoom(createRoomDto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/<RoomController>/5
        [HttpPut("ModifyRoom")]
        public async Task<IActionResult> ModifyRoom([FromBody] ModifyRoomDto modifyRoomDto)
        {
            var result = await _roomService.UpdateRoom(modifyRoomDto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/<RoomController>/5
        [HttpPut("DisableRoom")]
        public async Task<IActionResult> DeleteRoom([FromBody] DisableRoomDto disableRoomDto)
        {
            var result = await _roomService.DeleteRoom(disableRoomDto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}