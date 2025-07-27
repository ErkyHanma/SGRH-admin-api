using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.Floor.Response;
using SGRH.Web.Models.Hotel.Floor;
using System.Net.Http.Json; // Necesario para PostAsJsonAsync y PutAsJsonAsync

namespace SGRH.Web.Controllers
{
    public class FloorController : Controller
    {
        // GET: FloorController
        public async Task<IActionResult> Index()
        {
            GetAllFloorsResponse getAllFloorsResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync("Floor/GetFloors");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllFloorsResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllFloorsResponse>(responseString);
                    }
                    else
                    {
                        getAllFloorsResponse = new GetAllFloorsResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving floors."
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                getAllFloorsResponse = new GetAllFloorsResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving floors {ex.Message}."
                };
            }

            return View(getAllFloorsResponse.data);
        }



        // GET: FloorController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetFloorResponse getFloorResponse = null; 
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync($"Floor/GetFloorById?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getFloorResponse = System.Text.Json.JsonSerializer.Deserialize<GetFloorResponse>(responseString); 
                    }
                    else
                    {
                        getFloorResponse = new GetFloorResponse 
                        {
                            isSuccess = false,
                            message = "Error retrieving floor."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getFloorResponse = new GetFloorResponse 
                {
                    isSuccess = false,
                    message = $"Error retrieving floor {ex.Message}."
                };
            }
            return View(getFloorResponse.data);
        }

        // GET: FloorController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FloorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFloorModel createFloorModel)
        {
            CreateFloorResponse createResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PostAsJsonAsync("Floor/CreateFloor", createFloorModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    createResponse = System.Text.Json.JsonSerializer.Deserialize<CreateFloorResponse>(responseString);

                    if (createResponse != null && !createResponse.isSuccess)
                    {
                        ModelState.AddModelError("", createResponse.message);
                        return View(createFloorModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(createFloorModel);
            }
        }

        // GET: FloorController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetFloorResponse getFloorResponse = null; // Changed
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"Floor/GetFloorById?id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getFloorResponse = System.Text.Json.JsonSerializer.Deserialize<GetFloorResponse>(responseString); // Changed
                    }
                    else
                    {
                        getFloorResponse = new GetFloorResponse // Changed
                        {
                            isSuccess = false,
                            message = "Error retrieving floor."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getFloorResponse = new GetFloorResponse // Changed
                {
                    isSuccess = false,
                    message = $"Error retrieving floor {ex.Message}."
                };
            }
            if (getFloorResponse?.data == null)
                return NotFound();
            var editModel = new ModifyFloorModel
            {
                FloorId = getFloorResponse.data.FloorId,
                FloorNumber = getFloorResponse.data.FloorNumber,
                Description = getFloorResponse.data.Description,
                Status = getFloorResponse.data.Status
                
            };
            return View(editModel);
        }

        // POST: FloorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModifyFloorModel modifyFloorModel)
        {
            ModifyFloorResponse modifyResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PutAsJsonAsync("Floor/ModifyFloor", modifyFloorModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    modifyResponse = System.Text.Json.JsonSerializer.Deserialize<ModifyFloorResponse>(responseString);

                    if (modifyResponse != null && !modifyResponse.isSuccess)
                    {
                        ModelState.AddModelError("", modifyResponse.message);
                        return View(modifyFloorModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(modifyFloorModel);
            }
        }

        // GET: FloorController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            GetFloorResponse getFloorResponse = null; // Changed
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"Floor/GetFloorById?id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getFloorResponse = System.Text.Json.JsonSerializer.Deserialize<GetFloorResponse>(responseString); // Changed
                    }
                    else
                    {
                        getFloorResponse = new GetFloorResponse // Changed
                        {
                            isSuccess = false,
                            message = "Error retrieving floor."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getFloorResponse = new GetFloorResponse // Changed
                {
                    isSuccess = false,
                    message = $"Error retrieving floor {ex.Message}."
                };
            }
            if (getFloorResponse?.data == null)
                return NotFound();
            var deleteModel = new DisableFloorModel
            {
                FloorId = getFloorResponse.data.FloorId
                
            };
            return View(deleteModel);
        }

        // POST: FloorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, DisableFloorModel disableFloorModel)
        {
            DisableFloorResponse deleteResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                
                    var response = await client.PutAsJsonAsync("Floor/DisableFloor", disableFloorModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    deleteResponse = System.Text.Json.JsonSerializer.Deserialize<DisableFloorResponse>(responseString);

                    if (deleteResponse != null && !deleteResponse.isSuccess)
                    {
                        ModelState.AddModelError("", deleteResponse.message);
                        return View(disableFloorModel);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(disableFloorModel);
            }
        }
    }
}