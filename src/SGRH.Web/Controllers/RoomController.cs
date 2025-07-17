using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Entities.Hotel;

namespace SGRH.Web.Controllers
{
    public class RoomController : Controller
    {
        public readonly IRoomService _roomService;
        public RoomController(IRoomService roomService) 
        {
            _roomService = roomService; // break points aca 
        }

        //

        // GET: RoomController
        public async Task<IActionResult> Index()
        {
            var result = await _roomService.GetRooms(); // breakpoints en await

            if (result.IsSuccess)
            {
                List<Room> roomList = (List<Room>)result.Data;
                return View(roomList);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new Room());
            }
        }

        //

        // GET: RoomController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RoomController/Create
        public ActionResult Create() 
        {
            return View();
        }

        // POST: RoomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection) // ADD dto 
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

        // GET: RoomController/Edit/5
        public ActionResult Edit(int id) // se puede hacer copypaste al by id aqui (cuando lo tengamos)
        {
            return View();
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) //  MODIFY dto ??
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

        // GET: RoomController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) // disable dto
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
