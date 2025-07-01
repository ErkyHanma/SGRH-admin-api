using SGRH.Application.Dtos.Hotel.Rate;
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
    public interface IRatesService
    {
        // Expuesto al exterior en DTOs
        Task<OperationResult<IEnumerable<RateDto>>> GetRatesAsync();
        Task<OperationResult<RateDto>> GetRatesByIdAsync(int id);
        Task<OperationResult<CreateRateDto>> CreateRatesAsync(CreateRateDto dto);
        Task<OperationResult<RateDto>> UpdateRatesAsync(UpdateRateDto dto);
        Task<OperationResult<bool>> DeleteRatesAsync(DeleteRateDto dto);

        // opcional 
        //Task<bool> ExistsAsync(Expression<Func<Rate, bool>> filter);
    }

}
