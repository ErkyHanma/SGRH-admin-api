using SGRH.Application.Common.Logging;
using SGRH.Web.Infrastructure.Endpoints.Floor;
using SGRH.Web.Infrastructure.Http;
using SGRH.Web.Models.Hotel.Floor;
using SGRH.Web.Models.Hotel.Floor.Responses;
using SGRH.Web.Repositories.Base;
using SGRH.Web.Repositories.Interfaces.Hotel;

namespace SGRH.Web.Repositories
{
    public class FloorApiRepository : BaseApiRepository<FloorApiRepository>, IFloorApiRepository
    {
        private readonly IFloorEndpoints _endpoints; 

        public FloorApiRepository(
            IHttpClientService httpClientService,
            IAppLogger<FloorApiRepository> appLogger,
            IFloorEndpoints endpoints) 
            : base(httpClientService, appLogger)
        {
            _endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        }

        public async Task<GetAllFloorsResponse> GetFloorsAsync()
        {
            _appLogger.Info("Retrieving all floors");
            return await GetAsync<GetAllFloorsResponse>(_endpoints.GetAllFloors);
        }

        public async Task<GetFloorResponse> GetFloorByIdAsync(int id)
        {
            if (!IsValidId(id, "GetFloorById"))
                return new GetFloorResponse();

            _appLogger.Info("Retrieving floor with ID: {Id}", id);
            var endpoint = $"{_endpoints.GetFloorById}?id={id}";
            return await GetAsync<GetFloorResponse>(endpoint);
        }

        public Task<FloorCreateResponse> CreateFloorAsync(CreateFloorModel createFloorModel)
        {
            if (createFloorModel != null)
                _appLogger.Info("Creating new floor: {FloorNumber}", createFloorModel.FloorNumber); // Ajusta la propiedad según tu modelo

            return ExecuteApiCall<CreateFloorModel, FloorCreateResponse>(
                _endpoints.CreateFloor,
                createFloorModel,
                _httpClientService.PostAsync<FloorCreateResponse, CreateFloorModel>);
        }

        public Task<FloorEditResponse> EditFloorAsync(EditFloorModel editFloorModel)
        {
            if (editFloorModel != null)
                _appLogger.Info("Updating floor with ID: {FloorId}", editFloorModel.FloorId); // Ajusta la propiedad según tu modelo

            return ExecuteApiCall<EditFloorModel, FloorEditResponse>(
                _endpoints.ModifyFloor,
                editFloorModel,
                _httpClientService.PutAsync<FloorEditResponse, EditFloorModel>);
        }

        public Task<DeleteFloorResponse> DeleteFloorAsync(DeleteFloorModel deleteFloorModel)
        {
            if (deleteFloorModel != null)
                _appLogger.Info("Disabling floor with ID: {FloorId}", deleteFloorModel.FloorId); // Ajusta la propiedad según tu modelo

            return ExecuteApiCall<DeleteFloorModel, DeleteFloorResponse>(
                _endpoints.DisableFloor,
                deleteFloorModel,
                _httpClientService.PutAsync<DeleteFloorResponse, DeleteFloorModel>);
        }
    }
}