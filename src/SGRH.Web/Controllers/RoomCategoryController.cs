using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; 
using SGRH.Web.Models.Base;
using SGRH.Web.Models.Hotel.RoomCategory; 
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; 
using System.Text.Json;
using System.Threading.Tasks;

namespace SGRH.Web.Controllers
{
    public class RoomCategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public RoomCategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] + "RoomCategory";
        }

        // GET: RoomCategory
        public async Task<IActionResult> Index()
        {
            GetAllRoomCategoryResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetAllRoomCategoryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    responseModel = new GetAllRoomCategoryResponse { isSuccess = false, message = $"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}" };
                }
            }
            catch (HttpRequestException ex)
            {
                responseModel = new GetAllRoomCategoryResponse { isSuccess = false, message = $"Network Error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                responseModel = new GetAllRoomCategoryResponse { isSuccess = false, message = $"An unexpected error occurred: {ex.Message}" };
            }

            if (responseModel?.isSuccess == true && responseModel.data != null)
            {
                return View(responseModel.data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, responseModel?.message ?? "Failed to retrieve room categories.");
                return View(new List<RoomCategoryModel>());
            }
        }

        // GET: RoomCategory/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetRoomCategoryResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetRoomCategoryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound();
                    }
                    responseModel = new GetRoomCategoryResponse { isSuccess = false, message = $"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}" };
                }
            }
            catch (HttpRequestException ex)
            {
                responseModel = new GetRoomCategoryResponse { isSuccess = false, message = $"Network Error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                responseModel = new GetRoomCategoryResponse { isSuccess = false, message = $"An unexpected error occurred: {ex.Message}" };
            }

            if (responseModel?.isSuccess == true && responseModel.data != null)
            {
                return View(responseModel.data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, responseModel?.message ?? "Room category details not found.");
                return NotFound();
            }
        }

        // GET: RoomCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            BaseResponse<int>? createResponse = null; // Assuming API returns ID on success
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
                        ModelState.AddModelError(string.Empty, createResponse?.message ?? "Failed to create room category.");
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

        // GET: RoomCategory/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetRoomCategoryResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetRoomCategoryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                // Map to ModifyRoomCategoryModel for the view
                var modifyModel = new ModifyRoomCategoryModel
                {
                    CategoryId = responseModel.data.CategoryId,
                    Name = responseModel.data.Name,
                    Description = responseModel.data.Description,
                    MaxCapacity = responseModel.data.MaxCapacity,
                    Amenities = responseModel.data.Amenities
                };
                return View(modifyModel);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: RoomCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModifyRoomCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            BaseResponse<bool>? updateResponse = null; 
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/{model.CategoryId}", model);

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
                        ModelState.AddModelError(string.Empty, updateResponse?.message ?? "Failed to update room category.");
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

        // GET: RoomCategory/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            GetRoomCategoryResponse? responseModel = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonSerializer.Deserialize<GetRoomCategoryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                return View(responseModel.data); 
            }
            else
            {
                return NotFound();
            }
        }

        // POST: RoomCategory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int categoryId, int updatedBy) 
        {
            var model = new DisableRoomCategoryModel { CategoryId = categoryId, UpdatedBy = updatedBy };

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Delete), new { id = categoryId }); 
            }

            BaseResponse<bool>? deleteResponse = null; 
            try
            {
                // Assuming Disable uses a POST to /Disable
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
                        ModelState.AddModelError(string.Empty, deleteResponse?.message ?? "Failed to disable room category.");
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

            
            var originalCategoryResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/{categoryId}");
            if (originalCategoryResponse.IsSuccessStatusCode)
            {
                var content = await originalCategoryResponse.Content.ReadAsStringAsync();
                var originalCategoryData = JsonSerializer.Deserialize<GetRoomCategoryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (originalCategoryData?.isSuccess == true && originalCategoryData.data != null)
                {
                    return View("Delete", originalCategoryData.data); 
                }
            }
            return View("Delete", new RoomCategoryModel { CategoryId = categoryId }); 
        }
    }
}