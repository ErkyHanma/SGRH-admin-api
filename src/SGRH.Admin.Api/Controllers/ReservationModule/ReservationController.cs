using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Interfaces.Services.ReservationModule;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGRH.Api.Controllers.ReservationModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _reservationService.GetAllReservationAsync();

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _reservationService.GetReservationByIdAsync(id);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // POST api/<ReservationController>
        [HttpPost("CreateReservation")]
        public async Task<IActionResult> Post([FromBody] CreateReservationDto createReservationDto)
        {
            var result = await _reservationService.AddReservationAsync(createReservationDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/<ReservationController>/5
        [HttpPost("UpdateReservation")]
        public async Task<IActionResult> Post([FromBody] UpdateReservationDto updateReservationDto)
        {
            var result = await _reservationService.UpdateReservationAsync(updateReservationDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("DisableReservation")]
        public async Task<IActionResult> Post([FromBody] DisableReservationDto disableReservationDto)
        {
            var result = await _reservationService.DeleteReservationAsync(disableReservationDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("CheckAvailability")]
        public async Task<IActionResult> CheckAvailability([FromBody] int roomId, DateTime startDate, DateTime endDate)
        {
            var result = await _reservationService.CheckAvailability(roomId, startDate, endDate);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
