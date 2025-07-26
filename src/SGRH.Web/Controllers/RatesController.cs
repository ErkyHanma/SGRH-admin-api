using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGRH.Web.Models.Hotel.Rates.Responses;
using SGRH.Web.Models.Hotel.Rates;

namespace SGRH.Web.Controllers
{
    public class RatesController : Controller
    {
        // GET: RatesController
        public async Task<IActionResult> Index()
        {
            GetAllRatesResponse getAllRatesResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/");

                    var response = await client.GetAsync("api/Rate/GetRates"); 

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllRatesResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllRatesResponse>(responseString);
                    }
                    else
                    {
                        getAllRatesResponse = new GetAllRatesResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rates."
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                getAllRatesResponse = new GetAllRatesResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving rates {ex.Message}."
                };
            }

            //return View(getAllRatesResponse.Data);
            return View(getAllRatesResponse.data);

        }

        // GET: RatesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            //validar 

            GetRateResponse getRateResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/");

                    var response = await client.GetAsync($"/api/Rate/GetRateById?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRateResponse = System.Text.Json.JsonSerializer.Deserialize<GetRateResponse>(responseString);
                    }
                    else
                    {
                        getRateResponse = new GetRateResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rate."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRateResponse = new GetRateResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving rate {ex.Message}."
                };
            }
            return View(getRateResponse.data);
        }

        // GET: RatesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RatesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RateCreateModel rateCreateModel)
        {
            RateCreateResponse createResponse = null;

            try
            {
                rateCreateModel.createdAt = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/");

                    var response = await client.PostAsJsonAsync("/api/Rate/CreateRate", rateCreateModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    createResponse = System.Text.Json.JsonSerializer.Deserialize<RateCreateResponse>(responseString);

                    if (createResponse != null && !createResponse.isSuccess)
                    {
                        ModelState.AddModelError("", createResponse.message);
                        return View(rateCreateModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(rateCreateModel);
            }
        }

        // GET: RatesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetRateResponse getRateResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/");

                    var response = await client.GetAsync($"/api/Rate/GetRateById?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRateResponse = System.Text.Json.JsonSerializer.Deserialize<GetRateResponse>(responseString);
                    }
                    else
                    {
                        getRateResponse = new GetRateResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rate."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRateResponse = new GetRateResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving rate {ex.Message}."
                };
            }

            if (getRateResponse?.data == null)
                return NotFound();

            var editModel = new RateEditModel
            {
                rateId = getRateResponse.data.rateId,
                categoryId = getRateResponse.data.categoryId,
                seasonId = getRateResponse.data.seasonId,
                nightPrice = getRateResponse.data.nightPrice,
                isActive = getRateResponse.data.isActive,
                isDeleted = getRateResponse.data.isDeleted,
                createdAt = getRateResponse.data.createdAt,
                createdBy = getRateResponse.data.createdBy,
                updatedBy = getRateResponse.data.updatedBy,
                updatedAt = getRateResponse.data.updatedAt
            };

            return View(editModel);
        }


        // POST: RatesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RateEditModel rateEditModel) 
        {
            RateEditResponse editResponse = null;

            try
            {
                rateEditModel.updatedAt = DateTime.Now;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5171/");

                    var response = await client.PutAsJsonAsync("/api/Rate/UpdateRate", rateEditModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    editResponse = System.Text.Json.JsonSerializer.Deserialize<RateEditResponse>(responseString);

                    if (editResponse != null && !editResponse.isSuccess)
                    {
                        ModelState.AddModelError("", editResponse.message);
                        return View(rateEditModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(rateEditModel);
            }
        }

        // GET: RatesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            GetRateResponse getRateResponse = null;

            try
            {
                using (var client = new HttpClient()) 

                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.GetAsync($"Rate/GetRateById?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getRateResponse = System.Text.Json.JsonSerializer.Deserialize<GetRateResponse>(responseString);
                    }
                    else
                    {
                        getRateResponse = new GetRateResponse
                        {
                            isSuccess = false,
                            message = "Error retrieving rate."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getRateResponse = new GetRateResponse
                {
                    isSuccess = false,
                    message = $"Error retrieving rate {ex.Message}."
                };
            }

            if (getRateResponse?.data == null)
                return NotFound();

            var deleteModel = new RateDeleteModel
            {
                rateId = getRateResponse.data.rateId
            };

            return View(deleteModel);
        }

        // POST: RatesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, RateDeleteModel deleteRateModel)
        {
            DeleteRateResponse deleteResponse = null;

            try
            {
                using (var client = new HttpClient()) // http://localhost:5171/api/Rate/DeleteRate
                {
                    client.BaseAddress = new Uri("http://localhost:5171/api/");

                    var response = await client.PutAsJsonAsync("Rate/DeleteRate", deleteRateModel);

                    var responseString = await response.Content.ReadAsStringAsync();

                    deleteResponse = System.Text.Json.JsonSerializer.Deserialize<DeleteRateResponse>(responseString);

                    if (deleteResponse != null && !deleteResponse.isSuccess)
                    {
                        ModelState.AddModelError("", deleteResponse.message);
                        return View(deleteRateModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal error: " + ex.Message);
                return View(deleteRateModel);
            }
        }

    }
}

