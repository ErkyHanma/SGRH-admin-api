using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Domain.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.Services.Hotel
{
    public interface IFloorService
    {
        Task<OperationResult<IEnumerable<FloorDto>>> GetFloors();
        Task<OperationResult<FloorDto>> GetFloorsById(int floorId);
        Task<OperationResult<CreateFloorDto>> CreateFloor(CreateFloorDto createFloorDto);
        Task<OperationResult<ModifyFloorDto>> UpdateFloor(ModifyFloorDto modifyFloorDto);
        Task<OperationResult<DisableFloorDto>> DeleteFloor(DisableFloorDto disableFloorDto);
    }
}