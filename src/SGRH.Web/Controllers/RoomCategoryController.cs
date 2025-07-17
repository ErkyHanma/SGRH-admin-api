using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Entities.Hotel;

namespace SGRH.Web.Controllers
{
    public class RoomCategoryController : Controller
    {
        private readonly IRoomCategoryService _roomCategoryService;
        public RoomCategoryController(IRoomCategoryService roomCategoryService)
        {
            _roomCategoryService = roomCategoryService;
        }

        // GET: RoomCategoryController
        public async Task <ActionResult> Index()
        {
            var result = await _roomCategoryService.GetRoomCategories();
            if (result.IsSuccess)
            {
                List<RoomCategory> roomCategoriesList =(List<RoomCategory>)result.Data;
                return View(roomCategoriesList);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new RoomCategory());
            }
           
        }

        // GET: RoomCategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RoomCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoomCategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoomCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoomCategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoomCategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
