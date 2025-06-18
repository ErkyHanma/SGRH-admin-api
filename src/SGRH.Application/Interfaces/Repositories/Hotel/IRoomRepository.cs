using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Repositories.Hotel
{
    public interface IRoomRepository 
    {
        //Arreglados, faltan Dtos 

        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(CreateRoomDto createRoomDto);
        Task<OperationResult> UpdateAsync(ModifyRoomDto modifyRoomDto);
        Task<OperationResult> DeleteAsync(DisableRoomDto disableRoomDto);
        
        //Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        //Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);

    }
}
