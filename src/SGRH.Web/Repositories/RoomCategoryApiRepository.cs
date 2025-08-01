// SGRH.Web.Repositories/RoomCategoryApiRepository.cs
using SGRH.Application.Common.Logging;
using SGRH.Web.Infrastructure.Endpoints.RoomCategory;
using SGRH.Web.Infrastructure.Http;
using SGRH.Web.Models.Hotel.RoomCategory;
using SGRH.Web.Models.Hotel.RoomCategory.Responses;
using SGRH.Web.Repositories.Base;
using SGRH.Web.Repositories.Interfaces.Hotel;

namespace SGRH.Web.Repositories
{
    public class RoomCategoryApiRepository : BaseApiRepository<RoomCategoryApiRepository>, IRoomCategoryApiRepository
    {
        private readonly IRoomCategoryEndpoints _endpoints;

        public RoomCategoryApiRepository(
            IHttpClientService httpClientService,
            IAppLogger<RoomCategoryApiRepository> appLogger,
            IRoomCategoryEndpoints endpoints)
            : base(httpClientService, appLogger)
        {
            _endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        }

        public async Task<GetAllRoomCategoriesResponse> GetRoomCategoriesAsync()
        {
            _appLogger.Info("Retrieving all room categories");
            return await GetAsync<GetAllRoomCategoriesResponse>(_endpoints.GetAllRoomCategories);
        }

        public async Task<GetRoomCategoryResponse> GetRoomCategoryByIdAsync(int id)
        {
            if (!IsValidId(id, "GetRoomCategoryById"))
                return new GetRoomCategoryResponse();

            _appLogger.Info("Retrieving room category with ID: {Id}", id);
            var endpoint = $"{_endpoints.GetRoomCategoryById}?id={id}";
            return await GetAsync<GetRoomCategoryResponse>(endpoint);
        }

        public Task<RoomCategoryCreateResponse> CreateRoomCategoryAsync(CreateRoomCategoryModel createRoomCategoryModel)
        {
            if (createRoomCategoryModel != null)
                _appLogger.Info("Creating new room category: {CategoryName}", createRoomCategoryModel.Name);

            return ExecuteApiCall<CreateRoomCategoryModel, RoomCategoryCreateResponse>(
                _endpoints.CreateRoomCategory,
                createRoomCategoryModel,
                _httpClientService.PostAsync<RoomCategoryCreateResponse, CreateRoomCategoryModel>);
        }

        public Task<RoomCategoryEditResponse> EditRoomCategoryAsync(EditRoomCategoryModel editRoomCategoryModel)
        {
            if (editRoomCategoryModel != null)
                _appLogger.Info("Updating room category with ID: {RoomCategoryId}", editRoomCategoryModel.CategoryId);

            return ExecuteApiCall<EditRoomCategoryModel, RoomCategoryEditResponse>(
                _endpoints.ModifyRoomCategory,
                editRoomCategoryModel,
                _httpClientService.PutAsync<RoomCategoryEditResponse, EditRoomCategoryModel>);
        }

        public Task<DeleteRoomCategoryResponse> DeleteRoomCategoryAsync(DeleteRoomCategoryModel deleteRoomCategoryModel)
        {
            if (deleteRoomCategoryModel != null)
                _appLogger.Info("Disabling room category with ID: {RoomCategoryId}", deleteRoomCategoryModel.CategoryId);

            return ExecuteApiCall<DeleteRoomCategoryModel, DeleteRoomCategoryResponse>(
                _endpoints.DisableRoomCategory,
                deleteRoomCategoryModel,
                _httpClientService.PutAsync<DeleteRoomCategoryResponse, DeleteRoomCategoryModel>);
        }
    }
}