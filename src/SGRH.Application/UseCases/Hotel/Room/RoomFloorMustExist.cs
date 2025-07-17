using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.UseCases.Hotel.Room
{
    public class RoomFloorMustExist : IMustExistValidator<int>
    {
        private readonly IFloorRepository _floorRepository;
        public RoomFloorMustExist(IFloorRepository floorRepository)
        {
            _floorRepository = floorRepository;
        }

        public async Task<OperationResult<string>> Validate(int floorId)
        {
            var result = await _floorRepository.GetByIdAsync(floorId);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<string>.Failure($"FloorId {floorId} does not exist.");

            return OperationResult<string>.Success("Floor exists.");

        }
    }
}
