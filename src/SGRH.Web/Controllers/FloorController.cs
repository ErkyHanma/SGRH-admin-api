
using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.Floor;
using SGRH.Web.Models.Hotel.Floor.Responses;
using SGRH.Web.Repositories.Interfaces.Hotel;
using System.Threading.Tasks;

namespace SGRH.Web.Controllers
{
    [Route("Floor")]
    public class FloorController : Controller
    {
        private readonly IFloorApiRepository _floorApiRepository;

        public FloorController(IFloorApiRepository floorApiRepository)
        {
            _floorApiRepository = floorApiRepository;
        }

        // GET: Floor/Index 
        public async Task<IActionResult> Index()
        {
            GetAllFloorsResponse response = await _floorApiRepository.GetFloorsAsync();
            if (response.IsSuccess)
            {
                return View(response.Data); 
            }
            
            ViewBag.ErrorMessage = response.Message ?? "Error al obtener los pisos.";
            return View(new List<FloorModel>()); 
        }

        // GET: Floor/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            GetFloorResponse response = await _floorApiRepository.GetFloorByIdAsync(id);
            if (response.IsSuccess && response.Data != null)
            {
                return View(response.Data);
            }
           
            ViewBag.ErrorMessage = response.Message ?? "Piso no encontrado o error al obtenerlo.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Floor/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Floor/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateFloorModel model)
        {
            if (ModelState.IsValid)
            {
                FloorCreateResponse response = await _floorApiRepository.CreateFloorAsync(model);
                if (response.IsSuccess)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Piso creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", response.Message ?? "Error al crear el piso.");
            }
            return View(model);
        }

        // GET: Floor/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            GetFloorResponse response = await _floorApiRepository.GetFloorByIdAsync(id);
            if (response.IsSuccess && response.Data != null)
            {
                
                var editModel = new EditFloorModel
                {
                    FloorId = response.Data.FloorId,
                    FloorNumber = response.Data.FloorNumber,
                    Description = response.Data.Description,
                    Status = response.Data.Status
                    
                };
                return View(editModel);
            }
            ViewBag.ErrorMessage = response.Message ?? "Piso no encontrado para edición.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Floor/Edit
        [HttpPut("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] EditFloorModel model)
        {
            if (ModelState.IsValid)
            {
                FloorEditResponse response = await _floorApiRepository.EditFloorAsync(model);
                if (response.IsSuccess)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Piso actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", response.Message ?? "Error al actualizar el piso.");
            }
            return View(model);
        }

        // GET: Floor/Delete/5 
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            GetFloorResponse response = await _floorApiRepository.GetFloorByIdAsync(id);
            if (response.IsSuccess && response.Data != null)
            {

                var deleteModel = new DeleteFloorModel { FloorId = response.Data.FloorId };
                ViewBag.FloorNumber = response.Data.FloorNumber; 
                return View(deleteModel);
            }
            ViewBag.ErrorMessage = response.Message ?? "Piso no encontrado para deshabilitar.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Floor/Delete
        [HttpPut("DeleteConfirmed")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] DeleteFloorModel model)
        {
            DeleteFloorResponse response = await _floorApiRepository.DeleteFloorAsync(model);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = response.Message ?? "Piso deshabilitado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.Message ?? "Error al deshabilitar el piso.");
            // Si hay un error, redirige de nuevo a la vista de confirmación de eliminación o al Index
            return RedirectToAction(nameof(Delete), new { id = model.FloorId });
        }
    }
}