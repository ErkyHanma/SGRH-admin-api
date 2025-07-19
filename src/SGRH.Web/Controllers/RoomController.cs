using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Application.Dtos.Hotel.Room;
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

        // GET: RoomController 
        public async Task<IActionResult> Index()
        {
            var result = await _roomService.GetRooms(); // breakpoints en await

            if (result.IsSuccess)
            {
                List<RoomDto> roomList = (List<RoomDto>)result.Data;
                return View(roomList);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new Room());
            }
        }

        // GET: RoomController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _roomService.GetRoomsById(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            var room = result.Data; // RoomDto

            return View(room);
        }

        // GET: RoomController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomDto createRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createRoomDto); // regresa el formulario con errores
            }

            var result = await _roomService.CreateRoom(createRoomDto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(createRoomDto);
            }
        }

        // GET: RoomController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _roomService.GetRoomsById(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            var dto = new ModifyRoomDto
            {
                RoomId = result.Data.RoomId,
                RoomNumber = result.Data.RoomNumber,
                CategoryId = result.Data.CategoryId,
                FloorId = result.Data.FloorId,
                Status = result.Data.Status
            };

            return View(dto);
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModifyRoomDto modifyRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return View(modifyRoomDto);
            }

            var result = await _roomService.UpdateRoom(modifyRoomDto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(modifyRoomDto);
        }

        // GET: RoomController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.GetRoomsById(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            var disableDto = new DisableRoomDto
            {
                RoomId = id
                // UpdatedBy se completa en el formulario  
            };

            return View(disableDto);
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DisableRoomDto disableRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return View(disableRoomDto);
            }

            var result = await _roomService.DeleteRoom(disableRoomDto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(disableRoomDto);
        }
    }
}
