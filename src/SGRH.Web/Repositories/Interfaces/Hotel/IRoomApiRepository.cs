using SGRH.Web.Models.Hotel.Room;
using SGRH.Web.Models.Hotel.Room.Responses;

namespace SGRH.Web.Repositories.Interfaces.Hotel
{
    public interface IRoomApiRepository
    {
        Task<GetAllRoomsResponse> GetRoomsAsync();

        Task<GetRoomResponse> GetRoomByIdAsync(int id);

        Task<RoomCreateResponse> CreateRoomAsync(CreateRoomModel model);

        Task<RoomEditResponse> EditRoomAsync(EditRoomModel model);

        Task<DeleteRoomResponse> DeleteRoomAsync(DeleteRoomModel model);
    }
}
