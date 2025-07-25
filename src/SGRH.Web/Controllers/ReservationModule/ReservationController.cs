using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.ReservationModule;
using SGRH.Web.Models.ReservationModule.Response;


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
                        getAllReservationResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllReservationResponse>(responseString);
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
                        getReservationByIdResponse = System.Text.Json.JsonSerializer.Deserialize<GetReservationByIdResponse>(responseString);
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

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        createReservationResponse = System.Text.Json.JsonSerializer.Deserialize<CreateReservationResponse>(responseString);


                        if (createReservationResponse != null && createReservationResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
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
                        editReservationResponse = System.Text.Json.JsonSerializer.Deserialize<EditReservationResponse>(responseString);
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

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        editReservationResponse = System.Text.Json.JsonSerializer.Deserialize<EditReservationResponse>(responseString);


                        if (editReservationResponse != null && editReservationResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
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
                        deleteReservationResponse = System.Text.Json.JsonSerializer.Deserialize<DeleteReservationResponse>(responseString);
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

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        deleteReservationResponse = System.Text.Json.JsonSerializer.Deserialize<DeleteReservationResponse>(responseString);


                        if (deleteReservationResponse != null && deleteReservationResponse.isSuccess)
                        {
                            return RedirectToAction(nameof(Index));
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
