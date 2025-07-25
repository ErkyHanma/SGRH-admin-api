using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Controllers.ServiceModule
{
    public class ServiceController : Controller
    {



        // GET: ServiceController
        public async Task<IActionResult> Index()
        {
            GetAllServicesResponse getAllServiceResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync("Service");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllServiceResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllServicesResponse>(responseString);
                    }
                    else
                    {
                        getAllServiceResponse = new GetAllServicesResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rates."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getAllServiceResponse = new GetAllServicesResponse
                {
                    isSuccess = false,
                    message = "Error retrieving rates."
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

                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"Service/{id}");


                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getServiceByIdResponse = System.Text.Json.JsonSerializer.Deserialize<GetServiceByIdResponse>(responseString);
                    }
                    else
                    {
                        getServiceByIdResponse = new GetServiceByIdResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rates."
                        };
                    }


                }
            }
            catch (Exception ex)
            {
                getServiceByIdResponse = new GetServiceByIdResponse
                {
                    isSuccess = false,
                    message = "Error retrieving rates."
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
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.PostAsJsonAsync($"Service/CreateService", createServiceModel);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        ServiceResponse = System.Text.Json.JsonSerializer.Deserialize<BaseResponse<ServiceModel>>(responseString);

                        if (ServiceResponse != null && ServiceResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error retrieving rates.");
            }

            // If something went wrong, stay on form
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

                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"Service/{id}");


                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getServiceByIdResponse = System.Text.Json.JsonSerializer.Deserialize<GetServiceByIdResponse>(responseString);
                    }
                    else
                    {
                        getServiceByIdResponse = new GetServiceByIdResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rates."
                        };
                    }


                }
            }
            catch (Exception ex)
            {
                getServiceByIdResponse = new GetServiceByIdResponse
                {
                    isSuccess = false,
                    message = "Error retrieving rates."
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
                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.PostAsJsonAsync($"Service/UpdateService", serviceModel);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        editServiceResponse = System.Text.Json.JsonSerializer.Deserialize<EditServiceResponse>(responseString);

                        if (editServiceResponse != null && editServiceResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error retrieving rates.");
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

                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.GetAsync($"Service/{id}");


                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        deleteServiceResponse = System.Text.Json.JsonSerializer.Deserialize<DeleteServiceResponse>(responseString);
                    }
                    else
                    {
                        deleteServiceResponse = new DeleteServiceResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rates."
                        };
                    }


                }
            }
            catch (Exception ex)
            {
                deleteServiceResponse = new DeleteServiceResponse
                {
                    isSuccess = false,
                    message = "Error retrieving rates."
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

                    client.BaseAddress = new Uri("http://localhost:5171/api/");
                    var response = await client.PostAsJsonAsync($"Service/DisableService", deleteServiceModel);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        deleteServiceResponse = System.Text.Json.JsonSerializer.Deserialize<DeleteServiceResponse>(responseString);

                        if (deleteServiceResponse != null && deleteServiceResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }


                }

            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error retrieving rates.");
            }

            // If something went wrong, stay on form
            return View(deleteServiceModel);
        }
    }
}
