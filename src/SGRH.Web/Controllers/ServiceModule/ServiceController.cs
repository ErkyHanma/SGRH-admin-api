using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Interfaces.HttpClients.ServiceModule;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Controllers.ServiceModule
{
    public class ServiceController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IServiceHttpClient _serviceHttpClient;

        public ServiceController(IConfiguration configuration, IServiceHttpClient serviceHttpClient)
        {
            _configuration = configuration;
            _serviceHttpClient = serviceHttpClient;
        }

        // GET: ServiceController
        public async Task<IActionResult> Index()
        {
            GetAllServicesResponse getAllServiceResponse = null;

            try
            {
                getAllServiceResponse = await _serviceHttpClient.GetAllServicesAsync();

            }
            catch (Exception ex)
            {
                getAllServiceResponse = new GetAllServicesResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };
            }

            return View(getAllServiceResponse?.data ?? new List<ServiceModel>());
        }

        // GET: ServiceController/Details/5
        public async Task<IActionResult> Details(int id)
        {

            GetServiceByIdResponse getServiceByIdResponse = null;

            try
            {
                getServiceByIdResponse = await _serviceHttpClient.GetServiceByIdAsync(id);
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
            CreateServiceResponse createServiceResponse = null;

            try
            {
                createServiceResponse = await _serviceHttpClient.CreateServiceAsync(createServiceModel);

                if (createServiceResponse != null && createServiceResponse.isSuccess)
                {

                    return RedirectToAction(nameof(Index));
                }

                if (createServiceResponse != null && !string.IsNullOrEmpty(createServiceResponse.message))
                {
                    ModelState.AddModelError(string.Empty, createServiceResponse.message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the service.");
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
            EditServiceResponse editServiceResponse = null;

            try
            {
                editServiceResponse = await _serviceHttpClient.GetEditServiceByIdAsync(id);
            }
            catch (Exception ex)
            {
                editServiceResponse = new EditServiceResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving data. {ex.Message}"
                };

            }

            return View(editServiceResponse?.data ?? new ServiceModel());
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceModel serviceModel)
        {
            EditServiceResponse editServiceResponse = null;

            try
            {
                editServiceResponse = await _serviceHttpClient.EditServiceAsync(serviceModel);

                if (editServiceResponse != null && editServiceResponse.isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }


                if (editServiceResponse != null && !editServiceResponse.isSuccess && !string.IsNullOrEmpty(editServiceResponse.message))
                {
                    ModelState.AddModelError(string.Empty, editServiceResponse.message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the service.");
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
                deleteServiceResponse = await _serviceHttpClient.GetDeleteServiceByIdAsync(id);
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
                deleteServiceResponse = await _serviceHttpClient.DeleteServiceAsync(deleteServiceModel);

                if (deleteServiceResponse != null && deleteServiceResponse.isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }


                if (deleteServiceResponse != null && !deleteServiceResponse.isSuccess && !string.IsNullOrEmpty(deleteServiceResponse.message))
                {
                    ModelState.AddModelError(string.Empty, deleteServiceResponse.message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the service.");
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
