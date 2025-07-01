using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Interfaces.Services.Hotel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGRH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRatesService _rateService;

        public RateController(IRatesService rateService)
        {
            _rateService = rateService;
        }


        // GET: api/<RateController>
        [HttpGet("GetRates")]
        public async Task<IActionResult> GetRates()
        {
            var result = await _rateService.GetRatesAsync();
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET api/<RateController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRateById(int id)
        {
            var result = await _rateService.GetRatesByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // POST api/<RateController>
        [HttpPost("CreateRate")]
        public async Task<IActionResult> CreateRate([FromBody] CreateRateDto dto)
        {
            var result = await _rateService.CreateRatesAsync(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT api/<RateController>/5
        [HttpPut("UpdateRate")]
        public async Task<IActionResult> UpdateRate([FromBody] UpdateRateDto dto)
        {
            var result = await _rateService.UpdateRatesAsync(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // DELETE api/<RateController>/5
        [HttpPut("DeleteRate")]
        public async Task<IActionResult> DeleteRate([FromBody] DeleteRateDto dto)
        {
            var result = await _rateService.DeleteRatesAsync(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
