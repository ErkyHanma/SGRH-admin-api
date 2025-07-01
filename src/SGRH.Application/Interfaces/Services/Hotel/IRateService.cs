using SGRH.Domain.Base;
using SGRH.Domain.Entities.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.Services.Hotel
{
    public interface IRateService
    {
        Task<OperationResult<Rate>> AddAsync(Rate entity);
        Task<OperationResult<Rate>> UpdateAsync(Rate entity);
        Task<OperationResult<Rate>> DeleteAsync(Rate entity);
        Task<OperationResult<IEnumerable<Rate>>> GetAllAsync();
        Task<OperationResult<IEnumerable<Rate>>> GetAllAsync(Expression<Func<Rate, bool>> filter);
        Task<OperationResult<Rate>> GetByIdAsync(int id);
        Task<OperationResult<IEnumerable<Rate>>> GetRatesByCategoryAsync(int categoryId);
        Task<bool> ExistsAsync(Expression<Func<Rate, bool>> filter);
    }
}
