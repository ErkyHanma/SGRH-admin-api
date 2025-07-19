using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.Report.InputDtos;
using SGRH.Application.Interfaces.Services.Report;

namespace SGRH.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ---------- OccupancyReport ----------

        // GET: Reports/OccupancyReport
        public IActionResult OccupancyReport()
        {
            ViewData["ReportTitle"] = "Generate Occupancy Report";
            ViewData["FormAction"] = "GetOccupancyReport";
            return View("ReportDateRangeRequest", new ReportDateRangeRequestDto());
        }

        // POST: Reports/GetOccupancyReport
        [HttpPost]
        public async Task<IActionResult> GetOccupancyReport(ReportDateRangeRequestDto request)
        {
            var result = await _reportService.GetOcuppancyReport(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                ViewData["ReportTitle"] = "Generate Occupancy Report";
                ViewData["FormAction"] = "GetOccupancyReport";
                return View("ReportDateRangeRequest", request);
            }

            return View("OccupancyReportResult", result.Data);
        }

        // ---------- RatesReport ----------

        // GET: Reports/RatesReport
        public IActionResult RatesReport()
        {
            ViewData["ReportTitle"] = "Generate Rates Report";
            ViewData["FormAction"] = "GetRatesReport";
            return View("ReportDateRangeRequest", new ReportDateRangeRequestDto());
        }

        [HttpPost]
        public async Task<IActionResult> GetRatesReport(ReportDateRangeRequestDto request)
        {
            var result = await _reportService.GetRatesReport(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                ViewData["ReportTitle"] = "Generate Rates Report";
                ViewData["FormAction"] = "GetRatesReport";
                return View("ReportDateRangeRequest", request);
            }

            return View("RatesReportResult", result.Data);
        }

        // ---------- RevenueReport ----------

        // GET: Reports/RevenueReport
        public IActionResult RevenueReport()
        {
            ViewData["ReportTitle"] = "Generate Revenue Report";
            ViewData["FormAction"] = "GetRevenueReport";
            return View("ReportDateRangeRequest", new ReportDateRangeRequestDto());
        }

        [HttpPost]
        public async Task<IActionResult> GetRevenueReport(ReportDateRangeRequestDto request)
        {
            var result = await _reportService.GetRevenueReport(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                ViewData["ReportTitle"] = "Generate Revenue Report";
                ViewData["FormAction"] = "GetRevenueReport";
                return View("ReportDateRangeRequest", request);
            }

            return View("RevenueReportResult", result.Data);
        }

        // ---------- ServiceRevenueReport ----------

        // GET: Reports/ServiceRevenueReport
        public IActionResult ServiceRevenueReport()
        {
            return View("ServiceRevenueReportRequest", new ServiceRevenueRequestDto());
        }

        // POST: Reports/GetServiceRevenueReport
        [HttpPost]
        public async Task<IActionResult> GetServiceRevenueReport(ServiceRevenueRequestDto request)
        {
            var result = await _reportService.GetServiceRevenueReport(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View("ServiceRevenueReportRequest", request);
            }

            return View("ServiceRevenueReportResult", result.Data);
        }

    }
}
