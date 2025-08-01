using SGRH.Web.Models.Hotel.RoomCategory;
using SGRH.Web.Models.Hotel.RoomCategory.Responses;

namespace SGRH.Web.Repositories.Interfaces.Hotel
{
    public interface IRoomCategoryApiRepository
    {
        Task<GetAllRoomCategoriesResponse> GetRoomCategoriesAsync();

        Task<GetRoomCategoryResponse> GetRoomCategoryByIdAsync(int id);

        Task<RoomCategoryCreateResponse> CreateRoomCategoryAsync(CreateRoomCategoryModel model);

        Task<RoomCategoryEditResponse> EditRoomCategoryAsync(EditRoomCategoryModel model);

        Task<DeleteRoomCategoryResponse> DeleteRoomCategoryAsync(DeleteRoomCategoryModel model);
    }
}