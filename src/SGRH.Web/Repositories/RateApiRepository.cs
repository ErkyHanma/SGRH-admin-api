using SGRH.Web.Infrastructure.Http;
using SGRH.Web.Models.Hotel.Rates;
using SGRH.Web.Models.Hotel.Rates.Responses;
using SGRH.Web.Repositories.Base;
using SGRH.Web.Repositories.Interfaces.Hotel;
using SGRH.Web.Infrastructure.Endpoints.Rate;
using SGRH.Infrastructure.Common.Logging;

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

        public async Task<RateCreateResponse> CreateRateAsync(RateCreateModel rateCreateModel)
        {
            if (rateCreateModel == null)
            {
                _appLogger.ErrorNoEx("RateCreateModel cannot be null");
                return new RateCreateResponse();
            }

            rateCreateModel.createdAt = DateTime.Now;
            _appLogger.Info("Creating new rate for category: {CategoryId}", rateCreateModel.categoryId);

            return await PostAsync<RateCreateModel, RateCreateResponse>(_endpoints.CreateRate, rateCreateModel);
        }

        public async Task<RateEditResponse> EditRateAsync(RateEditModel rateEditModel)
        {
            if (rateEditModel == null)
            {
                _appLogger.ErrorNoEx("RateEditModel cannot be null");
                return new RateEditResponse();
            }

            rateEditModel.updatedAt = DateTime.Now;
            _appLogger.Info("Updating rate with ID: {RateId}", rateEditModel.rateId);

            return await PutAsync<RateEditModel, RateEditResponse>(_endpoints.UpdateRate, rateEditModel);
        }

        public async Task<DeleteRateResponse> DeleteRateAsync(RateDeleteModel rateDeleteModel)
        {
            if (rateDeleteModel == null)
            {
                _appLogger.ErrorNoEx("RateDeleteModel cannot be null");
                return new DeleteRateResponse();
            }

            _appLogger.Info("Deleting rate with ID: {RateId}", rateDeleteModel.rateId);
            return await PutAsync<RateDeleteModel, DeleteRateResponse>(_endpoints.DeleteRate, rateDeleteModel);
        }

    }
}
