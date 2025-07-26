using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;
using System.Text.Json;

namespace SGRH.Web.Controllers.ServiceModule
{
    public class ServiceController : Controller
    {

        private readonly IConfiguration _configuration;

        public ServiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: ServiceController
        public async Task<IActionResult> Index()
        {
            GetAllServicesResponse getAllServiceResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");

                    var response = await client.GetAsync("Service");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllServiceResponse = JsonSerializer.Deserialize<GetAllServicesResponse>(responseString);
                    }
                    else
                    {
                        getAllServiceResponse = new GetAllServicesResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getAllServiceResponse = new GetAllServicesResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };
            }


            var services = getAllServiceResponse?.data ?? new List<ServiceModel>();

            return View(services);
        }


        // GET: ServiceController/Details/5
        public async Task<IActionResult> Details(int id)
        {

            GetServiceByIdResponse getServiceByIdResponse = null;

            try
            {
                using (var client = new HttpClient())
                {

                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync($"Service/{id}");


                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getServiceByIdResponse = JsonSerializer.Deserialize<GetServiceByIdResponse>(responseString);
                    }
                    else
                    {
                        getServiceByIdResponse = new GetServiceByIdResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }


                }
            }
            catch (Exception ex)
            {
                getServiceByIdResponse = new GetServiceByIdResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }

            return View(getServiceByIdResponse?.data ?? new ServiceModel());
        }


        // GET: ServiceController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: ServiceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceModel createServiceModel)
        {
            BaseResponse<ServiceModel> ServiceResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");

                    var response = await client.PostAsJsonAsync("Service/CreateService", createServiceModel);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        ServiceResponse = JsonSerializer.Deserialize<BaseResponse<ServiceModel>>(responseContent);

                        if (ServiceResponse != null && ServiceResponse.isSuccess)
                        {
                            TempData["SuccessMessage"] = "Service created successfully.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        var errorResponse = JsonSerializer.Deserialize<BaseResponse<ServiceModel>>(responseContent);

                        if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.message))
                        {
                            ModelState.AddModelError(string.Empty, errorResponse.message);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "An error occurred while creating the service.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error: " + ex.Message);
            }

            // Return the same view with validation errors
            return View(createServiceModel);
        }


        // GET: ServiceController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetServiceByIdResponse getServiceByIdResponse = null;

            try
            {
                using (var client = new HttpClient())
                {

                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync($"Service/{id}");


                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getServiceByIdResponse = JsonSerializer.Deserialize<GetServiceByIdResponse>(responseString);
                    }
                    else
                    {
                        getServiceByIdResponse = new GetServiceByIdResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }


                }
            }
            catch (Exception ex)
            {
                getServiceByIdResponse = new GetServiceByIdResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }

            return View(getServiceByIdResponse?.data ?? new ServiceModel());
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceModel serviceModel)
        {
            EditServiceResponse editServiceResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.PostAsJsonAsync($"Service/UpdateService", serviceModel);

                    var responseString = await response.Content.ReadAsStringAsync();


                    if (response.IsSuccessStatusCode)
                    {
                        editServiceResponse = JsonSerializer.Deserialize<EditServiceResponse>(responseString);

                        if (editServiceResponse != null && editServiceResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        var errorResponse = JsonSerializer.Deserialize<BaseResponse<ServiceModel>>(responseString);

                        if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.message))
                        {
                            ModelState.AddModelError(string.Empty, errorResponse.message);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "An error occurred while creating the service.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error retrieving data.");
            }

            // If something went wrong, stay on form
            return View(serviceModel);
        }

        // GET: ServiceController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            DeleteServiceResponse deleteServiceResponse = null;

            try
            {
                using (var client = new HttpClient())
                {

                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync($"Service/{id}");


                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        deleteServiceResponse = JsonSerializer.Deserialize<DeleteServiceResponse>(responseString);
                    }
                    else
                    {
                        deleteServiceResponse = new DeleteServiceResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }


                }
            }
            catch (Exception ex)
            {
                deleteServiceResponse = new DeleteServiceResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }

            return View(deleteServiceResponse?.data ?? new DeleteServiceModel());
        }

        // POST: ServiceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteServiceModel deleteServiceModel)
        {
            DeleteServiceResponse deleteServiceResponse = null;

            try
            {

                using (var client = new HttpClient())
                {

                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.PostAsJsonAsync($"Service/DisableService", deleteServiceModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {

                        deleteServiceResponse = JsonSerializer.Deserialize<DeleteServiceResponse>(responseString);

                        if (deleteServiceResponse != null && deleteServiceResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        var errorResponse = JsonSerializer.Deserialize<BaseResponse<ServiceModel>>(responseString);

                        if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.message))
                        {
                            ModelState.AddModelError(string.Empty, errorResponse.message);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "An error occurred while creating the service.");
                        }

                    }


                }

            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error retrieving data.");
            }

            // If something went wrong, stay on form
            return View(deleteServiceModel);
        }
    }
}
