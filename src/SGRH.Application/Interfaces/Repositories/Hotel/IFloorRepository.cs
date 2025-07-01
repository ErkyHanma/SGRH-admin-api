using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Hotel
{
    public interface IFloorRepository
    {
        Task<OperationResult<IEnumerable<FloorDto>>> GetAllAsync();
        Task<OperationResult<FloorDto>> GetByIdAsync(int id);
        Task<OperationResult<CreateFloorDto>> AddAsync(CreateFloorDto createFloorDto);
        Task<OperationResult<ModifyFloorDto>> UpdateAsync(ModifyFloorDto modifyFloorDto);
        Task<OperationResult<DisableFloorDto>> DeleteAsync(DisableFloorDto disableFloorDto);
    }
}