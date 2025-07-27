using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.RoomCategory.Response;
using SGRH.Web.Models.Hotel.RoomCategory;
using System.Net.Http.Json; // Necesario para PostAsJsonAsync y PutAsJsonAsync

namespace SGRH.Web.Controllers
{
    public class RoomCategoryController : Controller
    {
        // GET: RoomCategoryController
        public async Task<IActionResult> Index()
        {
            GetAllRoomCategoriesResponse getAllRoomCategoriesResponse = null; //

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync("RoomCategory/GetRoomCategories");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllRoomCategoriesResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllRoomCategoriesResponse>(responseString); //
                    }
                    else
                    {
                        getAllRoomCategoriesResponse = new GetAllRoomCategoriesResponse //
                        {
                            isSuccess = false,
                            message = "Error retrieving room categories."
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                getAllRoomCategoriesResponse = new GetAllRoomCategoriesResponse //
                {
                    isSuccess = false,
                    message = $"Error retrieving room categories {ex.Message}."
                };
            }

            // Asumiendo que el campo 'data' en GetAllRoomCategoriesResponse contiene la lista
            return View(getAllRoomCategoriesResponse.data); //
        }

        // GET: RoomCategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetRoomCategoryResponse getRoomCategoryResponse = null; //
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync($"RoomCategory/GetRoomCategoryById?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRoomCategoryResponse = System.Text.Json.JsonSerializer.Deserialize<GetRoomCategoryResponse>(responseString); //
                    }
                    else
                    {
                        getRoomCategoryResponse = new GetRoomCategoryResponse //
                        {
                            isSuccess = false,
                            message = "Error retrieving room category."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRoomCategoryResponse = new GetRoomCategoryResponse //
                {
                    isSuccess = false,
                    message = $"Error retrieving room category {ex.Message}."
                };
            }
            // Asumiendo que el campo 'data' en GetRoomCategoryResponse contiene el objeto
            return View(getRoomCategoryResponse.data); //
        }

        // GET: RoomCategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomCategoryModel createRoomCategoryModel) //
        {
            CreateRoomCategoryResponse createResponse = null; //

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PostAsJsonAsync("RoomCategory/CreateRoomCategory", createRoomCategoryModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    createResponse = System.Text.Json.JsonSerializer.Deserialize<CreateRoomCategoryResponse>(responseString); //

                    if (createResponse != null && !createResponse.isSuccess) //
                    {
                        ModelState.AddModelError("", createResponse.message); //
                        return View(createRoomCategoryModel); //
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(createRoomCategoryModel); //
            }
        }

        // GET: RoomCategoryController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetRoomCategoryResponse getRoomCategoryResponse = null; //
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"RoomCategory/GetRoomCategoryById?id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRoomCategoryResponse = System.Text.Json.JsonSerializer.Deserialize<GetRoomCategoryResponse>(responseString); //
                    }
                    else
                    {
                        getRoomCategoryResponse = new GetRoomCategoryResponse //
                        {
                            isSuccess = false,
                            message = "Error retrieving room category."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRoomCategoryResponse = new GetRoomCategoryResponse //
                {
                    isSuccess = false,
                    message = $"Error retrieving room category {ex.Message}."
                };
            }
            if (getRoomCategoryResponse?.data == null) //
                return NotFound();

            // Mapear los datos obtenidos a ModifyRoomCategoryModel
            var editModel = new ModifyRoomCategoryModel //
            {
                CategoryId = getRoomCategoryResponse.data.CategoryId, //
                Name = getRoomCategoryResponse.data.Name, //
                Description = getRoomCategoryResponse.data.Description, //
                MaxCapacity = getRoomCategoryResponse.data.MaxCapacity, //
                Amenities = getRoomCategoryResponse.data.Amenities, //
                // UpdatedBy necesitará ser establecido, posiblemente desde la sesión o un valor predeterminado
            };
            return View(editModel); //
        }

        // POST: RoomCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModifyRoomCategoryModel modifyRoomCategoryModel) //
        {
            ModifyRoomCategoryResponse modifyResponse = null; //

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PutAsJsonAsync("RoomCategory/ModifyRoomCategory", modifyRoomCategoryModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    modifyResponse = System.Text.Json.JsonSerializer.Deserialize<ModifyRoomCategoryResponse>(responseString); //

                    if (modifyResponse != null && !modifyResponse.isSuccess) //
                    {
                        ModelState.AddModelError("", modifyResponse.message); //
                        return View(modifyRoomCategoryModel); //
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(modifyRoomCategoryModel); //
            }
        }

        // GET: RoomCategoryController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            GetRoomCategoryResponse getRoomCategoryResponse = null; //
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"RoomCategory/GetRoomCategoryById?id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRoomCategoryResponse = System.Text.Json.JsonSerializer.Deserialize<GetRoomCategoryResponse>(responseString); //
                    }
                    else
                    {
                        getRoomCategoryResponse = new GetRoomCategoryResponse //
                        {
                            isSuccess = false,
                            message = "Error retrieving room category."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRoomCategoryResponse = new GetRoomCategoryResponse //
                {
                    isSuccess = false,
                    message = $"Error retrieving room category {ex.Message}."
                };
            }
            if (getRoomCategoryResponse?.data == null) //
                return NotFound();

            // Mapear el ID a DisableRoomCategoryModel
            var deleteModel = new DisableRoomCategoryModel //
            {
                CategoryId = getRoomCategoryResponse.data.CategoryId //
                // UpdatedBy necesitará ser establecido, posiblemente desde la sesión o un valor predeterminado
            };
            return View(deleteModel); //
        }

        // POST: RoomCategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, DisableRoomCategoryModel disableRoomCategoryModel) //
        {
            DisableRoomCategoryResponse deleteResponse = null; //
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    // Asumiendo que la "eliminación" de una categoría de habitación es deshabilitarla
                    var response = await client.PutAsJsonAsync("RoomCategory/DisableRoomCategory", disableRoomCategoryModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    deleteResponse = System.Text.Json.JsonSerializer.Deserialize<DisableRoomCategoryResponse>(responseString); //

                    if (deleteResponse != null && !deleteResponse.isSuccess) //
                    {
                        ModelState.AddModelError("", deleteResponse.message); //
                        return View(disableRoomCategoryModel); //
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(disableRoomCategoryModel); //
            }
        }
    }
}