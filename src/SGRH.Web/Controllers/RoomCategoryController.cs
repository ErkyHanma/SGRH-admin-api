using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.RoomCategory;
using SGRH.Web.Models.Hotel.RoomCategory.Responses;
using SGRH.Web.Repositories.Interfaces.Hotel;
using System.Threading.Tasks;

namespace SGRH.Web.Controllers
{
    [Route("RoomCategory")]
    public class RoomCategoryController : Controller
    {
        private readonly IRoomCategoryApiRepository _roomCategoryApiRepository;

        public RoomCategoryController(IRoomCategoryApiRepository roomCategoryApiRepository)
        {
            _roomCategoryApiRepository = roomCategoryApiRepository;
        }

        // GET: RoomCategory/Index
        public async Task<IActionResult> Index()
        {
            GetAllRoomCategoriesResponse response = await _roomCategoryApiRepository.GetRoomCategoriesAsync();
            if (response.IsSuccess)
            {
                return View(response.Data);
            }
            ViewBag.ErrorMessage = response.Message ?? "Error al obtener las categorías de habitación.";
            return View(new List<RoomCategoryModel>());
        }

        // GET: RoomCategory/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            GetRoomCategoryResponse response = await _roomCategoryApiRepository.GetRoomCategoryByIdAsync(id);
            if (response.IsSuccess && response.Data != null) 
            {
                return View(response.Data);
            }
            ViewBag.ErrorMessage = response.Message ?? "Categoría de habitación no encontrada o error al obtenerla.";
            return RedirectToAction(nameof(Index));
        }

        // GET: RoomCategory/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomCategory/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateRoomCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                RoomCategoryCreateResponse response = await _roomCategoryApiRepository.CreateRoomCategoryAsync(model);
                if (response.IsSuccess)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Categoría de habitación creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", response.Message ?? "Error al crear la categoría de habitación.");
            }
            return View(model);
        }

        // GET: RoomCategory/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            GetRoomCategoryResponse response = await _roomCategoryApiRepository.GetRoomCategoryByIdAsync(id);
            if (response.IsSuccess && response.Data != null) 
            {
                var editModel = new EditRoomCategoryModel
                {
                    CategoryId = response.Data.CategoryId, 
                    Name = response.Data.Name, 
                    Description = response.Data.Description, 
                    MaxCapacity = response.Data.MaxCapacity, 
                    Amenities = response.Data.Amenities 
                };
                return View(editModel);
            }
            ViewBag.ErrorMessage = response.Message ?? "Categoría de habitación no encontrada para edición.";
            return RedirectToAction(nameof(Index));
        }

        // POST: RoomCategory/Edit
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] EditRoomCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                RoomCategoryEditResponse response = await _roomCategoryApiRepository.EditRoomCategoryAsync(model);
                if (response.IsSuccess)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Categoría de habitación actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", response.Message ?? "Error al actualizar la categoría de habitación.");
            }
            return View(model);
        }

        // GET: RoomCategory/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            GetRoomCategoryResponse response = await _roomCategoryApiRepository.GetRoomCategoryByIdAsync(id);
            if (response.IsSuccess && response.Data != null) 
            {
                var deleteModel = new DeleteRoomCategoryModel { CategoryId = response.Data.CategoryId };
                ViewBag.CategoryName = response.Data.Name;
                return View(deleteModel);
            }
            ViewBag.ErrorMessage = response.Message ?? "Categoría de habitación no encontrada para deshabilitar."; 
            return RedirectToAction(nameof(Index));
        }

        // POST: RoomCategory/DeleteConfirmed
        [HttpPost("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] DeleteRoomCategoryModel model)
        {
            DeleteRoomCategoryResponse response = await _roomCategoryApiRepository.DeleteRoomCategoryAsync(model);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = response.Message ?? "Categoría de habitación deshabilitada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.Message ?? "Error al deshabilitar la categoría de habitación.");
            return RedirectToAction(nameof(Delete), new { id = model.CategoryId });
        }
    }
}