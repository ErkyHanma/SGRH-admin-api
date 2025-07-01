using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Application.Interfaces.Services.Report;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGRH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase // Agregar validaciones?
    {
        private readonly IReportService _reportService;
        //private readonly IAppLogger<RoomController> _logger;
        //private readonly IConfiguration _configuration;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/<ReportController>
        [HttpGet("GetOcuppancyReport")]
        public async Task<IActionResult> GetOccupancyRep([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var request = new ReportDateRangeRequestDto { StartDate = startDate, EndDate = endDate};
            var result = await _reportService.GetOcuppancyReport(request);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetRatesReport")]
        public async Task<IActionResult> GetRatesRep([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var request = new ReportDateRangeRequestDto { StartDate = startDate, EndDate = endDate };
            var result = await _reportService.GetRatesReport(request);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetRevenueReport")]
        public async Task<IActionResult> GetRevenueRep([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var request = new ReportDateRangeRequestDto { StartDate = startDate, EndDate = endDate };
            var result = await _reportService.GetRevenueReport(request);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetServiceRevenueReport")]
        public async Task<IActionResult> GetServiceRevenueRep([FromQuery] int? categoryId)
        {
            var request = new ServiceRevenueRequestDto { CategoryId = categoryId };

            var result = await _reportService.GetServiceRevenueReport(request);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
