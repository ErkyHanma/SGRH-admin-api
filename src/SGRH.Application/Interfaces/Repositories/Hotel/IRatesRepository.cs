using SGRH.Domain.Base;
using SGRH.Domain.Entities.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.Repositories.Hotel
{
    public interface IRatesRepository : IBaseRepository<Rate>
    {
        Task<OperationResult<IEnumerable<Rate>>> GetRatesByCategoryAsync(int categoryId);
    }
}
