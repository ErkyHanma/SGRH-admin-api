using SGRH.Application.Common.Logging;
using SGRH.Web.Infrastructure.Endpoints.Rate;
using SGRH.Web.Infrastructure.Http;
using SGRH.Web.Models.Hotel.Room;
using SGRH.Web.Models.Hotel.Room.Responses;
using SGRH.Web.Repositories.Base;
using SGRH.Web.Repositories.Interfaces.Hotel;

namespace SGRH.Web.Repositories
{
    public class RoomApiRepository : BaseApiRepository<RoomApiRepository>, IRoomApiRepository
    {
        private readonly IRoomEndpoints _endpoints;

        public RoomApiRepository(
            IHttpClientService httpClientService,
            IAppLogger<RoomApiRepository> appLogger,
            IRoomEndpoints endpoints)
            : base(httpClientService, appLogger)
        {
            _endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        }

        public async Task<GetAllRoomsResponse> GetRoomsAsync()
        {
            _appLogger.Info("Retrieving all rooms");
            return await GetAsync<GetAllRoomsResponse>(_endpoints.GetAllRooms);
        }

        public async Task<GetRoomResponse> GetRoomByIdAsync(int id)
        {
            if (!IsValidId(id, "GetRoomById"))
                return new GetRoomResponse();

            _appLogger.Info("Retrieving room with ID: {Id}", id);
            var endpoint = $"{_endpoints.GetRoomById}?id={id}";
            return await GetAsync<GetRoomResponse>(endpoint);
        }

        public Task<RoomCreateResponse> CreateRoomAsync(CreateRoomModel createRoomModel)
        {
            if (createRoomModel != null)
                _appLogger.Info("Creating new room: {RoomNumber}", createRoomModel.roomNumber);

            return ExecuteApiCall<CreateRoomModel, RoomCreateResponse>(
                _endpoints.CreateRoom,
                createRoomModel,
                _httpClientService.PostAsync<RoomCreateResponse, CreateRoomModel>);
        }

        public Task<RoomEditResponse> EditRoomAsync(EditRoomModel editRoomModel)
        {
            if (editRoomModel != null)
                _appLogger.Info("Updating room with ID: {RoomId}", editRoomModel.roomId);

            return ExecuteApiCall<EditRoomModel, RoomEditResponse>(
                _endpoints.ModifyRoom,
                editRoomModel,
                _httpClientService.PutAsync<RoomEditResponse, EditRoomModel>);
        }

        public Task<DeleteRoomResponse> DeleteRoomAsync(DeleteRoomModel deleteRoomModel)
        {
            if (deleteRoomModel != null)
                _appLogger.Info("Disabling room with ID: {RoomId}", deleteRoomModel.roomId);

            return ExecuteApiCall<DeleteRoomModel, DeleteRoomResponse>(
                _endpoints.DisableRoom,
                deleteRoomModel,
                _httpClientService.PutAsync<DeleteRoomResponse, DeleteRoomModel>);
        }

    }
}