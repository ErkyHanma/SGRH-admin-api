using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models.ReservationModule.ReservationService;
using SGRH.Web.Models.ServiceModule;

namespace SGRH.Web.Controllers.ReservationModule
{
    public class ReservationServiceController : Controller
    {
        private readonly IReservationServiceHttpClient _reservationServiceHttpClient;

        public ReservationServiceController(IReservationServiceHttpClient reservationServiceHttpClient)
        {
            _reservationServiceHttpClient = reservationServiceHttpClient;
        }

        // GET: ReservationServiceController/Add/5
        public async Task<IActionResult> Add(int id)
        {
            AddReservationServiceModel model = null;

            try
            {
                var getAllServicesResponse = await _reservationServiceHttpClient.GetReservationServicesAsync(id);

                if (getAllServicesResponse.isSuccess)
                {
                    ViewBag.Services = getAllServicesResponse.data ?? new List<ServiceModel>();

                    model = new AddReservationServiceModel { reservationId = id };
                }

                if (getAllServicesResponse != null && !string.IsNullOrEmpty(getAllServicesResponse.message))
                {
                    ModelState.AddModelError(string.Empty, getAllServicesResponse.message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while retrieving the services.");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the services.");

            }

            return View(model ?? new AddReservationServiceModel());
        }

        // POST: ReservationServiceController/Add/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddReservationServiceModel addReservationServiceModel)
        {

            try
            {
                var addReservationServiceResponse = await _reservationServiceHttpClient.AddReservationAsync(addReservationServiceModel);


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


            return View(addReservationServiceModel);
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

            try
            {
                var deleteReservationServiceResponse = await _reservationServiceHttpClient.DeleteReservationAsync(deleteReservationServiceModel);


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


            return View(deleteReservationServiceModel);
        }

    }
}
