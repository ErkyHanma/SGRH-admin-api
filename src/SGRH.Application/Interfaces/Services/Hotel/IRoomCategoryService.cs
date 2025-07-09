using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Domain.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.Services.Hotel
{
    public interface IRoomCategoryService
    {
        Task<OperationResult<IEnumerable<RoomCategoryDto>>> GetRoomCategories();
        Task<OperationResult<RoomCategoryDto>> GetRoomCategoryById(int categoryId);
        Task<OperationResult<CreateRoomCategoryDto>> CreateRoomCategory(CreateRoomCategoryDto createRoomCategoryDto);
        Task<OperationResult<ModifyRoomCategoryDto>> UpdateRoomCategory(ModifyRoomCategoryDto modifyRoomCategoryDto);
        Task<OperationResult<DisableRoomCategoryDto>> DeleteRoomCategory(DisableRoomCategoryDto disableRoomCategoryDto);
    }
}