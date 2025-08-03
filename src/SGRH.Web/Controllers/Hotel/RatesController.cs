using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.Rates;
using SGRH.Web.Repositories.Interfaces.Hotel;


namespace SGRH.Web.Controllers.Hotel
{
    public class RatesController : Controller
    {

        private readonly IRateApiRepository _rateApiRepository;

        public RatesController(IRateApiRepository rateApiRepository)
        {
            _rateApiRepository = rateApiRepository ?? throw new ArgumentNullException(nameof(rateApiRepository));
        }

        // GET: RatesController
        public async Task<IActionResult> Index()
        {
            var response = await _rateApiRepository.GetRatesAsync();

            if (response?.isSuccess == false)
            {
                ViewBag.ErrorMessage = response.message ?? "Error retrieving rates";
                return View(new List<object>());
            }

            return View(response?.data ?? new List<RateModel>());

        }

        // GET: RatesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _rateApiRepository.GetRateByIdAsync(id);

            if (response?.isSuccess == false || response?.data == null)
            {
                ViewBag.ErrorMessage = response.message ?? "Rate not found";
                return View();
            }

            return View(response.data);

        }

        // GET: RatesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RatesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RateCreateModel rateCreateModel)
        {
            if(!ModelState.IsValid)
                return View(rateCreateModel);

            var response = await _rateApiRepository.CreateRateAsync(rateCreateModel);

            if (response?.isSuccess == false)
            {
                ModelState.AddModelError("", response.message ?? "Error creating rate");
                return View(rateCreateModel);
            }

            TempData["SuccessMessage"] = "Rate created successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: RatesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _rateApiRepository.GetRateByIdAsync(id);

            if (response?.isSuccess == false || response?.data == null)
            {
                return NotFound("Rate not found");
            }

            var editModel = new RateEditModel
            {
                rateId = response.data.rateId,
                categoryId = response.data.categoryId,
                seasonId = response.data.seasonId,
                nightPrice = response.data.nightPrice,
                isActive = response.data.isActive,
                isDeleted = response.data.isDeleted,
                createdAt = response.data.createdAt,
                createdBy = response.data.createdBy,
                updatedBy = response.data.updatedBy,
                updatedAt = response.data.updatedAt
            };

            return View(editModel);
        }


        // POST: RatesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RateEditModel rateEditModel)
        {
            if (!ModelState.IsValid)
                return View(rateEditModel);

            var response = await _rateApiRepository.EditRateAsync(rateEditModel);

            if (response?.isSuccess == false)
            {
                ModelState.AddModelError("", response.message ?? "Error updating rate");
                return View(rateEditModel);
            }

            TempData["SuccessMessage"] = "Rate updated successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: RatesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _rateApiRepository.GetRateByIdAsync(id);

            if (response?.isSuccess == false || response?.data == null)
            {
                return NotFound("Rate not found");
            }

            var deleteModel = new RateDeleteModel
            {
                rateId = response.data.rateId
            };

            return View(deleteModel);
        }

        // POST: RatesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, RateDeleteModel deleteRateModel)
        {
            if (!ModelState.IsValid)
                return View(deleteRateModel);

            var response = await _rateApiRepository.DeleteRateAsync(deleteRateModel);

            if (response?.isSuccess == false)
            {
                ModelState.AddModelError("", response?.message ?? "Error deleting rate");
                return View(deleteRateModel);
            }

            TempData["SuccessMessage"] = "Rate deleted successfully";
            return RedirectToAction(nameof(Index));

        }

    }
}

