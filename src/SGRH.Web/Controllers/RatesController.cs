using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Interfaces.Services.Hotel;

namespace SGRH.Web.Controllers
{
    public class RatesController : Controller
    {
        private readonly IRatesService _rateService;

        public RatesController(IRatesService rateService)
        {
            _rateService = rateService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _rateService.GetRatesAsync();

            if (result.IsSuccess)
            {
                List<RateDto> rates = result.Data.ToList();
                return View(rates);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<RateDto>());
            }
        }

        // GET: RateController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _rateService.GetRatesByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            var rate = result.Data; // RateDto

            return View(rate);
        }


        // GET: RateController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRateDto createRateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createRateDto);
            }

            var result = await _rateService.CreateRatesAsync(createRateDto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(createRateDto);
            }
        }

        // GET: RateController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _rateService.GetRatesByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            var dto = new UpdateRateDto
            {
                RateId = result.Data.RateId,
                CategoryId = result.Data.CategoryId,
                SeasonId = result.Data.SeasonId,
                NightPrice = result.Data.NightPrice,
                // UpdatedBy lo completa el usuario
            };

            return View(dto);
        }

        // POST: RateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRateDto updateRateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateRateDto);
            }

            var result = await _rateService.UpdateRatesAsync(updateRateDto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(updateRateDto);
        }

        // GET: RateController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _rateService.GetRatesByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            var deleteDto = new DeleteRateDto
            {
                RateId = id
                // DeletedBy se completará en la vista manualmente (o por backend si prefieres)
            };

            return View(deleteDto);
        }

        // POST: RateController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteRateDto deleteRateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(deleteRateDto);
            }

            var result = await _rateService.DeleteRatesAsync(deleteRateDto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(deleteRateDto);
        }

    }
}
