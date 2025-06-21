using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Hotel
{
    public interface IRoomRepository
    {
        Task<OperationResult<IEnumerable<RoomDto>>> GetAllAsync();
        Task<OperationResult<RoomDto>> GetByIdAsync(int id);
        Task<OperationResult<CreateRoomDto>> AddAsync(CreateRoomDto createRoomDto);
        Task<OperationResult<ModifyRoomDto>> UpdateAsync(ModifyRoomDto modifyRoomDto);
        Task<OperationResult<DisableRoomDto>> DeleteAsync(DisableRoomDto disableRoomDto);

    }
}


