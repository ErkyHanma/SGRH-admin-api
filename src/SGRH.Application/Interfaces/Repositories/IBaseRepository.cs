using SGRH.Domain.Base;
using System.Linq.Expressions;

namespace SGRH.Application.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<OperationResult<IEnumerable<TEntity>>> GetAllAsync();
        Task<OperationResult<TEntity>> GetByIdAsync(int id);
        Task<OperationResult<TEntity>> AddAsync(TEntity entity);
        Task<OperationResult<TEntity>> UpdateAsync(TEntity entity);
        Task<OperationResult<TEntity>> DeleteAsync(TEntity entity);
        Task<OperationResult<IEnumerable<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    }
}

