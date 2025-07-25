using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ReservationModule.ReservationService.Response;
using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Controllers.ReservationModule
{
    public class ReservationServiceController : Controller
    {
        private readonly IConfiguration _configuration;

        public ReservationServiceController(IConfiguration configuration)
        {
            _configuration = configuration;
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
                        getAllServicesResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllServicesResponse>(responseString);

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
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.PostAsJsonAsync("ReservationService/AddReservationService", addReservationServiceModel);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        addReservationServiceResponse = System.Text.Json.JsonSerializer.Deserialize<AddReservationServiceResponse>(responseString);


                        if (addReservationServiceResponse != null && addReservationServiceResponse.isSuccess)
                        {
                            return RedirectToAction("Index", "Reservation");

                        }
                    }

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
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.PostAsJsonAsync("ReservationService/DeleteReservationService", deleteReservationServiceModel);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        deleteReservationServiceResponse = System.Text.Json.JsonSerializer.Deserialize<DeleteReservationServiceResponse>(responseString);


                        if (deleteReservationServiceResponse != null && deleteReservationServiceResponse.isSuccess)
                        {
                            return RedirectToAction("Index", "Reservation");

                        }
                    }

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
