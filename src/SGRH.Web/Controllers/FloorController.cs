using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SGRH.Web.Models.Base;
using SGRH.Web.Models.Hotel.Floor; 
using System.Collections.Generic; 
using System.Net.Http;
using System.Net.Http.Json; 
using System.Text.Json;
using System.Threading.Tasks;

namespace SGRH.Web.Controllers
{
    public class FloorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public FloorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] + "Floor";
        }

        public async Task<IActionResult> Index()
        {
            GetAllFloorResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetAllFloorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    
                    responseModel = new GetAllFloorResponse { isSuccess = false, message = $"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}" };
                }
            }
            catch (HttpRequestException ex) 
            {
                responseModel = new GetAllFloorResponse { isSuccess = false, message = $"Network Error: {ex.Message}" };
            }
            catch (Exception ex) 
            {
                responseModel = new GetAllFloorResponse { isSuccess = false, message = $"An unexpected error occurred: {ex.Message}" };
            }

            if (responseModel?.isSuccess == true && responseModel.data != null)
            {
                return View(responseModel.data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, responseModel?.message ?? "Failed to retrieve floors.");
                return View(new List<FloorModel>()); 
            }
        }

        // GET: Floor/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetFloorResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetFloorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound();
                    }
                    responseModel = new GetFloorResponse { isSuccess = false, message = $"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}" };
                }
            }
            catch (HttpRequestException ex)
            {
                responseModel = new GetFloorResponse { isSuccess = false, message = $"Network Error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                responseModel = new GetFloorResponse { isSuccess = false, message = $"An unexpected error occurred: {ex.Message}" };
            }

            if (responseModel?.isSuccess == true && responseModel.data != null)
            {
                return View(responseModel.data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, responseModel?.message ?? "Floor details not found.");
                return NotFound();
            }
        }

        // GET: Floor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Floor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFloorModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            BaseResponse<int>? createResponse = null; 
            try
            {
                
                var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, model);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    createResponse = JsonSerializer.Deserialize<BaseResponse<int>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (createResponse?.isSuccess == true)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, createResponse?.message ?? "Failed to create floor.");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Network Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            return View(model);
        }

        // GET: Floor/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetFloorResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetFloorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound();
                    }
                    ModelState.AddModelError(string.Empty, $"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Network Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            if (responseModel?.isSuccess == true && responseModel.data != null)
            {
                
                var modifyModel = new ModifyFloorModel
                {
                    FloorId = responseModel.data.FloorId,
                    FloorNumber = responseModel.data.FloorNumber,
                    Description = responseModel.data.Description,
                    Status = responseModel.data.Status 
                };
                return View(modifyModel);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Floor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModifyFloorModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            BaseResponse<bool>? updateResponse = null; 
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/{model.FloorId}", model);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    updateResponse = JsonSerializer.Deserialize<BaseResponse<bool>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (updateResponse?.isSuccess == true)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateResponse?.message ?? "Failed to update floor.");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Network Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            return View(model);
        }

        // GET: Floor/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            GetFloorResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetFloorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound();
                    }
                    ModelState.AddModelError(string.Empty, $"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Network Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            if (responseModel?.isSuccess == true && responseModel.data != null)
            {
                return View(responseModel.data); // Pass FloorModel to display details
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Floor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int floorId, int updatedBy) 
        {
            var model = new DisableFloorModel { FloorId = floorId, UpdatedBy = updatedBy };

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Delete), new { id = floorId }); 
            }

            BaseResponse<bool>? deleteResponse = null; 
            try
            {
                
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Disable", model); 

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    deleteResponse = JsonSerializer.Deserialize<BaseResponse<bool>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (deleteResponse?.isSuccess == true)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, deleteResponse?.message ?? "Failed to delete floor.");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Network Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            var originalFloorResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/{floorId}");
            if (originalFloorResponse.IsSuccessStatusCode)
            {
                var content = await originalFloorResponse.Content.ReadAsStringAsync();
                var originalFloorData = JsonSerializer.Deserialize<GetFloorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (originalFloorData?.isSuccess == true && originalFloorData.data != null)
                {
                    return View("Delete", originalFloorData.data); // Pass original model back to the Delete view
                }
            }
            return View("Delete", new FloorModel { FloorId = floorId }); // Fallback with minimal info
        }
    }
}