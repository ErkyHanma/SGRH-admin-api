using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using System.Threading.Tasks;
using System.Linq; 

namespace SGRH.Application.UseCases.Hotel.RoomCategory
{
    public class RoomCategoryNameMustBeUnique
    {
        private readonly IRoomCategoryRepository _roomCategoryRepository;

        public RoomCategoryNameMustBeUnique(IRoomCategoryRepository roomCategoryRepository)
        {
            _roomCategoryRepository = roomCategoryRepository;
        }

        // Método para validar la unicidad al crear una nueva categoría
        public async Task<OperationResult<string>> ValidateCreate(string categoryName)
        {
            var existingCategoriesResult = await _roomCategoryRepository.GetAllAsync(); 
            if (existingCategoriesResult.IsSuccess && existingCategoriesResult.Data != null)
            {
                if (existingCategoriesResult.Data.Any(rc => rc.Name != null && rc.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
                {
                    return OperationResult<string>.Failure($"Room category name '{categoryName}' already exists.");
                }
            }
            return OperationResult<string>.Success("Room category name is unique.");
        }

        // Método para validar la unicidad al modificar una categoría (excluyendo la propia categoría)
        public async Task<OperationResult<string>> ValidateModify(int categoryId, string categoryName)
        {
            var existingCategoriesResult = await _roomCategoryRepository.GetAllAsync(); 
            if (existingCategoriesResult.IsSuccess && existingCategoriesResult.Data != null)
            {
                if (existingCategoriesResult.Data.Any(rc => rc.Name != null && rc.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase) && rc.CategoryId != categoryId))
                {
                    return OperationResult<string>.Failure($"Room category name '{categoryName}' already exists for another category.");
                }
            }
            return OperationResult<string>.Success("Room category name is unique for modification.");
        }
    }
}