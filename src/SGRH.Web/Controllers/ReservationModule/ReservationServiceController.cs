using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ReservationModule.ReservationService.Response;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;
using System.Text.Json;

namespace SGRH.Web.Controllers.ReservationModule
{
    public class ReservationServiceController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IReservationServiceHttpClient _reservationServiceHttpClient;

        public ReservationServiceController(IConfiguration configuration, IReservationServiceHttpClient reservationServiceHttpClient)
        {
            _configuration = configuration;
            _reservationServiceHttpClient = reservationServiceHttpClient;
        }

        // GET: ReservationServiceController/Add/5
        public async Task<IActionResult> Add(int id)
        {
            GetAllServicesResponse getAllServicesResponse = null;
            AddReservationServiceModel model = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync($"Service");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllServicesResponse = JsonSerializer.Deserialize<GetAllServicesResponse>(responseString);

                        ViewBag.Services = getAllServicesResponse.data ?? new List<ServiceModel>();

                        model = new AddReservationServiceModel
                        {
                            reservationId = id
                        };

                    }
                    else
                    {
                        getAllServicesResponse = new GetAllServicesResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getAllServicesResponse = new GetAllServicesResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }

            return View(model ?? new AddReservationServiceModel());
        }

        // POST: ReservationServiceController/Add/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddReservationServiceModel addReservationServiceModel)
        {
            AddReservationServiceResponse addReservationServiceResponse = null;

            try
            {

                addReservationServiceResponse = await _reservationServiceHttpClient.AddReservationAsync(addReservationServiceModel);


                if (addReservationServiceResponse != null && addReservationServiceResponse.isSuccess)
                {
                    return RedirectToAction("Index", "Reservation");

                }


                if (addReservationServiceResponse != null && !string.IsNullOrEmpty(addReservationServiceResponse.message))
                {
                    ModelState.AddModelError(string.Empty, addReservationServiceResponse.message);
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


            return View(addReservationServiceResponse);
        }


        // GET: ReservationServiceController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = new DeleteReservationServiceModel
            {
                reservationId = id
            };
            return View(model);
        }

        // POST: ReservationServiceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteReservationServiceModel deleteReservationServiceModel)
        {

            DeleteReservationServiceResponse deleteReservationServiceResponse = null;

            try
            {
                deleteReservationServiceResponse = await _reservationServiceHttpClient.DeleteReservationAsync(deleteReservationServiceModel);


                if (deleteReservationServiceResponse != null && deleteReservationServiceResponse.isSuccess)
                {
                    return RedirectToAction("Index", "Reservation");

                }

                if (deleteReservationServiceResponse != null && !string.IsNullOrEmpty(deleteReservationServiceResponse.message))
                {
                    ModelState.AddModelError(string.Empty, deleteReservationServiceResponse.message);
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


            return View(deleteReservationServiceResponse);
        }

    }
}
