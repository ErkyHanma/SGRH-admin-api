using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.Room.Responses;
using SGRH.Web.Models.Hotel.Room;
using SGRH.Web.Models.Hotel.Rates.Responses;
using SGRH.Web.Models.Hotel.Rates;

namespace SGRH.Web.Controllers
{
    public class RoomController : Controller
    {
        // GET: RoomController
        public async Task<IActionResult> Index()
        {
            GetAllRoomsResponse getAllRoomsResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync("Room/GetRooms");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllRoomsResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllRoomsResponse>(responseString);
                    }
                    else
                    {
                        getAllRoomsResponse = new GetAllRoomsResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rooms."
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                getAllRoomsResponse = new GetAllRoomsResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving rooms {ex.Message}."
                };
            }

            return View(getAllRoomsResponse.data);
        }

        // GET: RoomController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetRoomResponse getRoomResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync($"Room/GetRoomById?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRoomResponse = System.Text.Json.JsonSerializer.Deserialize<GetRoomResponse>(responseString);
                    }
                    else
                    {
                        getRoomResponse = new GetRoomResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving room."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRoomResponse = new GetRoomResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving room {ex.Message}."
                };
            }
            return View(getRoomResponse.data);
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
            RoomCreateResponse createResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PostAsJsonAsync("Room/CreateRoom", createRoomModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    createResponse = System.Text.Json.JsonSerializer.Deserialize<RoomCreateResponse>(responseString);

                    if (createResponse != null && !createResponse.isSuccess)
                    {
                        ModelState.AddModelError("", createResponse.message);
                        return View(createRoomModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(createRoomModel);
            }
        }

        // GET: RoomController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetRoomResponse getRoomResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"Room/GetRoomById?id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRoomResponse = System.Text.Json.JsonSerializer.Deserialize<GetRoomResponse>(responseString);
                    }
                    else
                    {
                        getRoomResponse = new GetRoomResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving room."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRoomResponse = new GetRoomResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving room {ex.Message}."
                };
            }
            if (getRoomResponse?.data == null)
                return NotFound();
            var editModel = new EditRoomModel
            {
                roomId = getRoomResponse.data.roomId,
                roomNumber = getRoomResponse.data.roomNumber,
                categoryId = getRoomResponse.data.categoryId,
                floorId = getRoomResponse.data.floorId,
                description = getRoomResponse.data.description,
                roomImgUrl = getRoomResponse.data.roomImgUrl,
                status = getRoomResponse.data.status
            };
            return View(editModel);
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoomModel editRoomModel)
        {
            RoomEditResponse editResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PutAsJsonAsync("Room/ModifyRoom", editRoomModel);

                    var responseString = await response.Content.ReadAsStringAsync(); 

                    editResponse = System.Text.Json.JsonSerializer.Deserialize<RoomEditResponse>(responseString);

                    if (editResponse != null && !editResponse.isSuccess)
                    {
                        ModelState.AddModelError("", editResponse.message);
                        return View(editRoomModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(editRoomModel);
            }
        }

        // GET: RoomController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            GetRoomResponse getRoomResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"Room/GetRoomById?id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRoomResponse = System.Text.Json.JsonSerializer.Deserialize<GetRoomResponse>(responseString);
                    }
                    else
                    {
                        getRoomResponse = new GetRoomResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving room."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRoomResponse = new GetRoomResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving room {ex.Message}."
                };
            }
            if (getRoomResponse?.data == null)
                return NotFound();
            var deleteModel = new DeleteRoomModel
            {
                roomId = getRoomResponse.data.roomId
            };
            return View(deleteModel);
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, DeleteRoomModel deleteRoomModel)
        {
            DeleteRoomResponse deleteResponse = null;
            try
            {
                using (var client = new HttpClient()) // http://localhost:5171/api/Room/DeleteRoom
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PutAsJsonAsync("Room/DisableRoom", deleteRoomModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    deleteResponse = System.Text.Json.JsonSerializer.Deserialize<DeleteRoomResponse>(responseString);

                    if (deleteResponse != null && !deleteResponse.isSuccess)
                    {
                        ModelState.AddModelError("", deleteResponse.message);
                        return View(deleteRoomModel);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(deleteRoomModel);
            }
        }
    }
}