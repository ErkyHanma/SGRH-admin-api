using SGRH.Application.Common.Logging;
using SGRH.Web.Infrastructure.Http;
using SGRH.Web.Infrastructure.Endpoints;
using SGRH.Web.Models.Hotel.Rates;
using SGRH.Web.Models.Hotel.Rates.Responses;
using SGRH.Web.Repositories.Base;
using SGRH.Web.Repositories.Interfaces.Hotel;
using SGRH.Web.Infrastructure.Endpoints.Rate;

namespace SGRH.Web.Repositories
{
    public class RateApiRepository : BaseApiRepository<RateApiRepository>, IRateApiRepository
    {
        private readonly IRateEndpoints _endpoints;

        public RateApiRepository(
            IHttpClientService httpClientService,
            IAppLogger<RateApiRepository> appLogger,
            IRateEndpoints endpoints)
            : base(httpClientService, appLogger)
        {
            _endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        }

        public async Task<GetAllRatesResponse> GetRatesAsync()
        {
            _appLogger.Info("Retrieving all rates");
            return await GetAsync<GetAllRatesResponse>(_endpoints.GetAllRates);
        }

        public async Task<GetRateResponse> GetRateByIdAsync(int id)
        {
            if (!IsValidId(id, "GetRateById"))
                return new GetRateResponse();

            _appLogger.Info("Retrieving rate with ID: {Id}", id);
            var endpoint = $"{_endpoints.GetRateById}?id={id}";
            return await GetAsync<GetRateResponse>(endpoint);
        }

        public Task<RateCreateResponse> CreateRateAsync(RateCreateModel rateCreateModel)
        {
            if (rateCreateModel != null)
            {
                rateCreateModel.createdAt = DateTime.Now;
                _appLogger.Info("Creating new rate for category: {CategoryId}", rateCreateModel.categoryId);
            }

            return ExecuteApiCall<RateCreateModel, RateCreateResponse>(
                _endpoints.CreateRate,
                rateCreateModel,
                _httpClientService.PostAsync<RateCreateResponse, RateCreateModel>);
        }

        public Task<RateEditResponse> EditRateAsync(RateEditModel rateEditModel)
        {
            if (rateEditModel != null)
            {
                rateEditModel.updatedAt = DateTime.Now;
                _appLogger.Info("Updating rate with ID: {RateId}", rateEditModel.rateId);
            }

            return ExecuteApiCall<RateEditModel, RateEditResponse>(
                _endpoints.UpdateRate,
                rateEditModel,
                _httpClientService.PutAsync<RateEditResponse, RateEditModel>);
        }

        public async Task<DeleteRateResponse> DeleteRateAsync(RateDeleteModel rateDeleteModel)
        {
            if (rateDeleteModel != null)
                _appLogger.Info("Deleting rate with ID: {RateId}", rateDeleteModel.rateId);

            return await ExecuteApiCall<RateDeleteModel, DeleteRateResponse>(
                _endpoints.DeleteRate,
                rateDeleteModel,
                _httpClientService.PutAsync<DeleteRateResponse, RateDeleteModel>);
        }
    }
}
