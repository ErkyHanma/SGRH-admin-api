using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.Room.Responses;
using SGRH.Web.Models.Hotel.Room;
using SGRH.Web.Models.Hotel.Rates.Responses;
using SGRH.Web.Models.Hotel.Rates;
using SGRH.Web.Repositories;
using SGRH.Persistence.Repositories.Hotel;
using SGRH.Web.Repositories.Interfaces.Hotel;

namespace SGRH.Web.Controllers.Hotel
{
    public class RoomController : Controller
    {

        private readonly IRoomApiRepository _roomApiRepository;

        public RoomController(IRoomApiRepository roomApiRepository)
        {
            _roomApiRepository = roomApiRepository ?? throw new ArgumentNullException(nameof(roomApiRepository));
        }


        // GET: RoomController
        public async Task<IActionResult> Index()
        {
            var response = await _roomApiRepository.GetRoomsAsync();

            if (response?.isSuccess == false)
            {
                ViewBag.ErrorMessage = response.message ?? "Error retrieving rooms";
                return View(new List<object>());
            }

            return View(response?.data ?? new List<RoomModel>());
        }

        // GET: RoomController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _roomApiRepository.GetRoomByIdAsync(id);

            if (response?.isSuccess == false || response?.data == null)
            {
                ViewBag.ErrorMessage = response?.message ?? "Room not found";
                return View();
            }

            return View(response.data);
        }

        // GET: RoomController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomModel createRoomModel)
        {
            if (!ModelState.IsValid)
                return View(createRoomModel);

            var response = await _roomApiRepository.CreateRoomAsync(createRoomModel);

            if (response?.isSuccess == false)
            {
                ModelState.AddModelError("", response.message ?? "Error creating room");
                return View(createRoomModel);
            }

            TempData["SuccessMessage"] = "Room created successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: RoomController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _roomApiRepository.GetRoomByIdAsync(id);

            if (response?.isSuccess == false || response?.data == null)
            {
                return NotFound("Room not found");
            }

            var editModel = new EditRoomModel
            {
                roomId = response.data.roomId,
                roomNumber = response.data.roomNumber,
                categoryId = response.data.categoryId,
                floorId = response.data.floorId,
                description = response.data.description,
                roomImgUrl = response.data.roomImgUrl,
                status = response.data.status
            };

            return View(editModel);
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoomModel editRoomModel)
        {
            if (!ModelState.IsValid)
                return View(editRoomModel);

            var response = await _roomApiRepository.EditRoomAsync(editRoomModel);

            if (response?.isSuccess == false)
            {
                ModelState.AddModelError("", response.message ?? "Error editing room");
                return View(editRoomModel);
            }

            TempData["SuccessMessage"] = "Room edited successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: RoomController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _roomApiRepository.GetRoomByIdAsync(id);

            if (response?.isSuccess == false || response?.data == null)
            {
                return NotFound("Room not found");
            }

            var deleteModel = new DeleteRoomModel
            {
                roomId = response.data.roomId
            };

            return View(deleteModel);
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, DeleteRoomModel deleteRoomModel)
        {
            if (!ModelState.IsValid)
                return View(deleteRoomModel);

            var response = await _roomApiRepository.DeleteRoomAsync(deleteRoomModel);

            if (response?.isSuccess == false || response?.data == null)
            {
                ModelState.AddModelError("", response.message ?? "Error deleting room");
                return View(deleteRoomModel);
            }

            TempData["SuccessMessage"] = "Room deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}