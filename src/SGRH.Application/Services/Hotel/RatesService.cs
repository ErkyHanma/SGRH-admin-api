using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Services.Report;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Services.Hotel
{
    public sealed class RatesService : IRateService
    {
        private readonly IRateService _rateService;
        private readonly IAppLogger<RatesService> _logger;
        private readonly IConfiguration _configuration;

        public RatesService(IRateService rateService, IAppLogger<RatesService> logger, IConfiguration configuration)
        {
            _rateService = rateService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<Rate>> AddAsync(Rate entity)
        {
            _logger.Info("Creating rate category {0}", entity.CategoryId);

            if (entity is null)
            {
                return OperationResult<Rate>.Failure("Rate entity is required.");
            }

            if (entity.CategoryId <= 0 || entity.SeasonId <= 0 || entity.NightPrice <= 0 || entity.CreatedBy <= 0)
            {
                return OperationResult<Rate>.Failure("Invalid Rate data. CategoryId, SeasonId, NightPrice, and CreatedBy are required and must be greater than 0.");
            }

            return await _rateService.AddAsync(entity);
        }

        public async Task<OperationResult<Rate>> DeleteAsync(Rate entity)
        {
            _logger.Info("Deleting rate {0}", entity.RateId);

            if (entity is null || entity.RateId <= 0 || entity.DeletedBy <= 0)
            {
                return OperationResult<Rate>.Failure("RateId and DeletedBy are required and must be greater than 0.");
            }

            return await _rateService.DeleteAsync(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Rate, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<IEnumerable<Rate>>> GetAllAsync()
        {
            return await _rateService.GetAllAsync();
        }

        public async Task<OperationResult<IEnumerable<Rate>>> GetAllAsync(Expression<Func<Rate, bool>> filter)
        {
            return await _rateService.GetAllAsync(filter);
        }

        public async Task<OperationResult<Rate>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return OperationResult<Rate>.Failure("RateId must be greater than 0.");
            }

            return await _rateService.GetByIdAsync(id);

        }

        public async Task<OperationResult<IEnumerable<Rate>>> GetRatesByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                return OperationResult<IEnumerable<Rate>>.Failure("RateId must be greater than 0.");
            }

            return await _rateService.GetRatesByCategoryAsync(categoryId);
        }

        public async Task<OperationResult<Rate>> UpdateAsync(Rate entity)
        {
            if (entity is null)
            {
                return OperationResult<Rate>.Failure("Rate entity is required.");
            }
            

            if (entity.RateId <= 0 || entity.CategoryId <= 0 || entity.SeasonId <= 0 
                                   || entity.NightPrice <= 0 || entity.UpdatedBy <= 0)
            {
                return OperationResult<Rate>.Failure("Invalid Rate data. RateId, CategoryId, SeasonId, NightPrice, and UpdatedBy are required and must be greater than 0.");
            }


            _logger.Info("Updating rate {0}", entity.RateId);
            return await _rateService.UpdateAsync(entity);
        }
    }
}
