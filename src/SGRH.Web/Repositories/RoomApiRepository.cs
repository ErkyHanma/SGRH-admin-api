using SGRH.Infrastructure.Common.Logging;
using SGRH.Web.Infrastructure.Endpoints.Room;
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

        public async Task<RoomCreateResponse> CreateRoomAsync(CreateRoomModel createRoomModel)
        {
            if (createRoomModel == null)
            {
                _appLogger.ErrorNoEx("CreateRoomModel cannot be null");
                return new RoomCreateResponse();
            }

            _appLogger.Info("Creating new room: {RoomNumber}", createRoomModel.roomNumber);
            return await PostAsync<CreateRoomModel, RoomCreateResponse>(_endpoints.CreateRoom, createRoomModel);
        }

        public async Task<RoomEditResponse> EditRoomAsync(EditRoomModel editRoomModel)
        {
            if (editRoomModel == null)
            {
                _appLogger.ErrorNoEx("EditRoomModel cannot be null");
                return new RoomEditResponse();
            }

            _appLogger.Info("Updating room with ID: {RoomId}", editRoomModel.roomId);
            return await PutAsync<EditRoomModel, RoomEditResponse>(_endpoints.ModifyRoom, editRoomModel);
        }

        public async Task<DeleteRoomResponse> DeleteRoomAsync(DeleteRoomModel deleteRoomModel)
        {
            if (deleteRoomModel == null)
            {
                _appLogger.ErrorNoEx("DeleteRoomModel cannot be null");
                return new DeleteRoomResponse();
            }

            _appLogger.Info("Disabling room with ID: {RoomId}", deleteRoomModel.roomId);
            return await PutAsync<DeleteRoomModel, DeleteRoomResponse>(_endpoints.DisableRoom, deleteRoomModel);
        }
    }
}