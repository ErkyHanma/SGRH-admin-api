using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.ReservationModule.ReservationService;
using SGRH.Application.Interfaces.Services.ReservationModule;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGRH.Api.Controllers.ReservationModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationServiceController : ControllerBase
    {
        private readonly IReservationServiceService _reservationServiceService;
        public ReservationServiceController(IReservationServiceService reservationServiceService)
        {
            _reservationServiceService = reservationServiceService;
        }


        // POST api/<ReservationController>
        [HttpPost("CreateReservation")]
        public async Task<IActionResult> Post([FromBody] CreateReservationServiceDto createReservationServiceDto)
        {
            var result = await _reservationServiceService.AddReservationServiceAsync(createReservationServiceDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("DisableReservation")]
        public async Task<IActionResult> Post([FromBody] DeleteReservationServiceDto deleteReservationServiceDto)
        {
            var result = await _reservationServiceService.DeleteReservationServiceAsync(deleteReservationServiceDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
