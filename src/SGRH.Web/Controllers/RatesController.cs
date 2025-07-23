using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Web.Models;

namespace SGRH.Web.Controllers
{
    public class RatesController : Controller
    {
        private readonly IRatesService _rateService;

        public RatesController(IRatesService rateService)
        {
            _rateService = rateService;
        }

        // usando Model y HttpClient
        public async Task<IActionResult> Index()
        {
            GetAllRatesResponse response = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5171/");

                var apiResponse = await client.GetAsync("api/Rate/GetRates");

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = System.Text.Json.JsonSerializer.Deserialize<GetAllRatesResponse>(responseString);
                }
                else
                {
                    response = new GetAllRatesResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving rates."
                    };
                }
            }

            return View(response?.data ?? new List<RateModel>());
        }

        // usando Model y HttpClient
        public async Task<IActionResult> Details(int id)
        {
            GetRateResponse response = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5171/");

                var apiResponse = await client.GetAsync($"api/Rate/GetRateById?id={id}");

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = System.Text.Json.JsonSerializer.Deserialize<GetRateResponse>(responseString);
                }
                else
                {
                    response = new GetRateResponse
                    {
                        isSuccess = false,
                        message = "Error retrieving rate."
                    };
                }
            }

            return View(response?.data);
        }

        // usando DTOs y Service Layer
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            dto.CreatedAt = DateTime.Now;

            var result = await _rateService.CreateRatesAsync(dto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // usando DTOs y Service Layer
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _rateService.GetRatesByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound();

            var dto = new UpdateRateDto
            {
                RateId = result.Data.RateId,
                CategoryId = result.Data.CategoryId,
                SeasonId = result.Data.SeasonId,
                NightPrice = result.Data.NightPrice
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            dto.UpdatedAt = DateTime.Now;

            var result = await _rateService.UpdateRatesAsync(dto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
