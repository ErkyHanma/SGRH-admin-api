using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models;
using SGRH.Web.Models.ReservationModule.Reservation;
using SGRH.Web.Models.ReservationModule.Reservation.Response;
using SGRH.Web.Models.ServiceModule;
using System.Text.Json;

namespace SGRH.Web.Controllers.ReservationModule
{
    public class ReservationController : Controller
    {

        private readonly IConfiguration _configuration;

        public ReservationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //GET: ReservationController
        public async Task<IActionResult> Index()
        {
            GetAllReservationResponse getAllReservationResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync("Reservation");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllReservationResponse = JsonSerializer.Deserialize<GetAllReservationResponse>(responseString);
                    }
                    else
                    {
                        getAllReservationResponse = new GetAllReservationResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getAllReservationResponse = new GetAllReservationResponse
                {
                    isSuccess = false,
                    message = "Error retrieving data."
                };

            }


            return View(getAllReservationResponse?.data ?? new List<ReservationModel>());


        }

        //GET: ReservationController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetReservationByIdResponse getReservationByIdResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync($"Reservation/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getReservationByIdResponse = JsonSerializer.Deserialize<GetReservationByIdResponse>(responseString);
                    }
                    else
                    {
                        getReservationByIdResponse = new GetReservationByIdResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }
                }
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
            CreateReservationResponse createReservationResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.PostAsJsonAsync("Reservation/CreateReservation", createReservationModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        createReservationResponse = JsonSerializer.Deserialize<CreateReservationResponse>(responseString);


                        if (createReservationResponse != null && createReservationResponse.isSuccess)
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


            return View(createReservationModel);
        }

        // GET: ReservationController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            EditReservationResponse editReservationResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync($"Reservation/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        editReservationResponse = JsonSerializer.Deserialize<EditReservationResponse>(responseString);
                    }
                    else
                    {
                        editReservationResponse = new EditReservationResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }
                }
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
            EditReservationResponse editReservationResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.PostAsJsonAsync("Reservation/UpdateReservation", editReservationModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        editReservationResponse = JsonSerializer.Deserialize<EditReservationResponse>(responseString);


                        if (editReservationResponse != null && editReservationResponse.isSuccess)
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


            return View(editReservationResponse);
        }

        // GET: ReservationController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            DeleteReservationResponse deleteReservationResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.GetAsync($"Reservation/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        deleteReservationResponse = JsonSerializer.Deserialize<DeleteReservationResponse>(responseString);
                    }
                    else
                    {
                        deleteReservationResponse = new DeleteReservationResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving data."
                        };
                    }
                }
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
            DeleteReservationResponse deleteReservationResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(baseUrl ?? "");
                    var response = await client.PostAsJsonAsync("Reservation/DisableReservation", deleteReservationModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        deleteReservationResponse = JsonSerializer.Deserialize<DeleteReservationResponse>(responseString);


                        if (deleteReservationResponse != null && deleteReservationResponse.isSuccess)
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


            return View(deleteReservationResponse);
        }




    }
}
