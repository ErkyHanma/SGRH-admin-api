using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Hotel
{
    public interface IRoomCategoryRepository
    {
        Task<OperationResult<IEnumerable<RoomCategoryDto>>> GetAllAsync();
        Task<OperationResult<RoomCategoryDto>> GetByIdAsync(int id);
        Task<OperationResult<CreateRoomCategoryDto>> AddAsync(CreateRoomCategoryDto createRoomCategoryDto);
        Task<OperationResult<ModifyRoomCategoryDto>> UpdateAsync(ModifyRoomCategoryDto modifyRoomCategoryDto);
        Task<OperationResult<DisableRoomCategoryDto>> DeleteAsync(DisableRoomCategoryDto disableRoomCategoryDto);
    }
}