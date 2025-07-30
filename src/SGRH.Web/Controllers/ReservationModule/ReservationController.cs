using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Interfaces.HttpClients.ReservationModule;
using SGRH.Web.Models.ReservationModule.Reservation;
using SGRH.Web.Models.ReservationModule.Reservation.Response;

namespace SGRH.Web.Controllers.ReservationModule
{
    public class ReservationController : Controller
    {

        private readonly IReservationHttpClient _reservationHttpClient;

        public ReservationController(IReservationHttpClient reservationHttpClient)
        {
            _reservationHttpClient = reservationHttpClient;
        }

        //GET: ReservationController
        public async Task<IActionResult> Index()
        {
            GetAllReservationResponse getAllReservationResponse = null;

            try
            {
                getAllReservationResponse = await _reservationHttpClient.GetAllReservationAsync();

            }
            catch (Exception ex)
            {
                getAllReservationResponse = new GetAllReservationResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving data.{ex.Message}"
                };

            }


            return View(getAllReservationResponse?.data ?? []);


        }

        //GET: ReservationController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetReservationByIdResponse getReservationByIdResponse = null;

            try
            {
                getReservationByIdResponse = await _reservationHttpClient.GetReservationByIdAsync(id);
            }
            catch (Exception ex)
            {
                getReservationByIdResponse = new GetReservationByIdResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }


            return View(getReservationByIdResponse?.data ?? new ReservationModel());
        }

        // GET: ReservationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationModel createReservationModel)
        {

            try
            {
                var createReservationResponse = await _reservationHttpClient.CreateReservationAsync(createReservationModel);

                if (createReservationResponse != null && createReservationResponse.isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (createReservationResponse != null && !string.IsNullOrEmpty(createReservationResponse.message))
                {
                    ModelState.AddModelError(string.Empty, createReservationResponse.message);
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


            return View(createReservationModel);
        }

        // GET: ReservationController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            EditReservationResponse editReservationResponse = null;

            try
            {
                editReservationResponse = await _reservationHttpClient.GetEditReservationByIdAsync(id);
            }
            catch (Exception ex)
            {
                editReservationResponse = new EditReservationResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }

            return View(editReservationResponse?.data ?? new EditReservationModel());

        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditReservationModel editReservationModel)
        {

            try
            {
                var editReservationResponse = await _reservationHttpClient.EditReservationAsync(editReservationModel);


                if (editReservationResponse != null && editReservationResponse.isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (editReservationResponse != null && !string.IsNullOrEmpty(editReservationResponse.message))
                {
                    ModelState.AddModelError(string.Empty, editReservationResponse.message);
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


            return View(editReservationModel);
        }

        // GET: ReservationController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            DeleteReservationResponse deleteReservationResponse = null;

            try
            {
                deleteReservationResponse = await _reservationHttpClient.GetDeleteReservationByIdAsync(id);
            }
            catch (Exception ex)
            {
                deleteReservationResponse = new DeleteReservationResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }

            return View(deleteReservationResponse?.data ?? new DeleteReservationModel());
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteReservationModel deleteReservationModel)
        {

            try
            {
                var deleteReservationResponse = await _reservationHttpClient.DeleteReservationAsync(deleteReservationModel);


                if (deleteReservationResponse != null && deleteReservationResponse.isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (deleteReservationResponse != null && !string.IsNullOrEmpty(deleteReservationResponse.message))
                {
                    ModelState.AddModelError(string.Empty, deleteReservationResponse.message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the service.");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error retrieving data. {ex.Message}");
            }


            return View(deleteReservationModel);
        }


    }
}
