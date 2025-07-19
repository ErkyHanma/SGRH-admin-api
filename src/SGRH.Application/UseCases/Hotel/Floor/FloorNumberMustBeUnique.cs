using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using System.Threading.Tasks;
using System.Linq; 
using SGRH.Application.Interfaces.UseCases; 

namespace SGRH.Application.UseCases.Hotel.Floor
{
    public class FloorNumberMustBeUnique : IMustBeUniqueValidator<int>
    {
        private readonly IFloorRepository _floorRepository;

        public FloorNumberMustBeUnique(IFloorRepository floorRepository)
        {
            _floorRepository = floorRepository;
        }

        // Método para validar la unicidad al crear un nuevo piso
        public async Task<OperationResult<string>> ValidateCreate(int floorNumber)
        {
            var existingFloorResult = await _floorRepository.GetAllAsync(); // Obtener todos los pisos
            if (existingFloorResult.IsSuccess && existingFloorResult.Data != null)
            {
                if (existingFloorResult.Data.Any(f => f.FloorNumber == floorNumber))
                {
                    return OperationResult<string>.Failure($"Floor number {floorNumber} already exists.");
                }
            }
            return OperationResult<string>.Success("Floor number is unique.");
        }

        // Método para validar la unicidad al modificar un piso (excluyendo el propio piso)
        public async Task<OperationResult<string>> ValidateModify(int floorId, int floorNumber)
        {
            var existingFloorResult = await _floorRepository.GetAllAsync(); // Obtener todos los pisos
            if (existingFloorResult.IsSuccess && existingFloorResult.Data != null)
            {
                if (existingFloorResult.Data.Any(f => f.FloorNumber == floorNumber && f.FloorId != floorId))
                {
                    return OperationResult<string>.Failure($"Floor number {floorNumber} already exists for another floor.");
                }
            }
            return OperationResult<string>.Success("Floor number is unique for modification.");
        }
    }
}