using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using System.Threading.Tasks;
using System.Linq; // Necesario para .Any()

namespace SGRH.Application.UseCases.Hotel.RoomCategory
{
    public class RoomCategoryMustNotHaveAssociatedRooms
    {
        private readonly IRoomRepository _roomRepository; // Inyectar el repositorio de Habitaciones
        private readonly IRoomCategoryRepository _roomCategoryRepository; // Para verificar si la categoría existe

        public RoomCategoryMustNotHaveAssociatedRooms(IRoomRepository roomRepository, IRoomCategoryRepository roomCategoryRepository)
        {
            _roomRepository = roomRepository;
            _roomCategoryRepository = roomCategoryRepository;
        }

        public async Task<OperationResult<string>> Validate(int categoryId)
        {
            // Paso 1: Verificar si la categoría existe
            var categoryResult = await _roomCategoryRepository.GetByIdAsync(categoryId);
            if (!categoryResult.IsSuccess || categoryResult.Data == null)
            {
                return OperationResult<string>.Failure($"Room category ID {categoryId} does not exist.");
            }

            // Paso 2: Verificar si hay habitaciones asociadas a esta categoría
            var allRoomsResult = await _roomRepository.GetAllAsync();
            if (allRoomsResult.IsSuccess && allRoomsResult.Data != null)
            {
                // Verifica si alguna habitación tiene el CategoryId especificado y está activa
                if (allRoomsResult.Data.Any(r => r.CategoryId == categoryId && r.Status != "deleted" && r.Status != "inactive")) // Asumiendo que "deleted" e "inactive" son estados no asociados
                {
                    return OperationResult<string>.Failure($"Room category ID {categoryId} cannot be deleted because it has associated active rooms.");
                }
            }
            // Si hay un error al obtener las habitaciones, o si no hay datos, no lo consideramos un error de asociación aquí.
            return OperationResult<string>.Success("Room category has no active associated rooms.");
        }
    }
}