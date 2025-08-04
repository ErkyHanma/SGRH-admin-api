using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Common.Common;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.Hotel;
using SGRH.Persistence.Context;
using System.Linq.Expressions;

namespace SGRH.Persistence.Repositories.Hotel
{
    public class RatesRepository : IRatesRepository
    {
        private readonly SGRHContext _context;
        private readonly IAppLogger<RatesRepository> _logger;

        public RatesRepository(SGRHContext context, IAppLogger<RatesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult<Rate>> AddAsync(Rate entity)
        {
            try
            {
                _logger.Info("Adding new rate entity");

                // Validaciones 

                if (entity == null)
                    return OperationResult<Rate>.Failure("Rate is null.");

                entity.CreatedAt = DateTime.Now.Date;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _context.Rate.AddAsync(entity);
                await _context.SaveChangesAsync();

                return OperationResult<Rate>.Success("Rate added successfully.", entity);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error adding rate.");
                return OperationResult<Rate>.Failure("Error occurred while adding rate.");
            }
        }

        public async Task<OperationResult<Rate>> DeleteAsync(Rate entity)
        {
            try
            {
                if (entity == null)
                    return OperationResult<Rate>.Failure("Rate is null.");

                var existing = await _context.Rate
                    .FirstOrDefaultAsync(r => r.RateId == entity.RateId && !r.IsDeleted && r.IsActive);

                if (existing == null)
                    return OperationResult<Rate>.Failure("Rate not found or already deleted.");

                existing.IsActive = false;
                existing.IsDeleted = true;
                existing.DeletedBy = entity.DeletedBy;
                existing.DeleteAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return OperationResult<Rate>.Success("Rate deleted successfully.", existing);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error deleting rate");
                return OperationResult<Rate>.Failure("Error occurred while deleting rate.");
            }
        }
        public async Task<bool> ExistsAsync(Expression<Func<Rate, bool>> filter)
        {
            return await _context.Rate.AnyAsync(filter);
        }
        public async Task<OperationResult<IEnumerable<Rate>>> GetAllAsync()
        {
            try
            {
                var rates = await _context.Rate // Obtener todas las tarifas que no estan eliminadas
                    .Where(r => !r.IsDeleted) // Filtra todos los registros que no sean IsDeleted
                    .ToListAsync();

                return OperationResult<IEnumerable<Rate>>.Success("Rates retrieved successfully.", rates);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error fetching all rates");
                return OperationResult<IEnumerable<Rate>>.Failure("Error fetching rates.");
            }
        }
        public async Task<OperationResult<IEnumerable<Rate>>> GetAllAsync(Expression<Func<Rate, bool>> filter)
        {
            try
            {
                var rates = await _context.Rate
                    .Where(filter) // Aplicar un filtro y obtener todas las tarifas no eliminadas 
                    .Where(r => !r.IsDeleted)
                    .ToListAsync();

                return OperationResult<IEnumerable<Rate>>.Success("Filtered rates retrieved successfully.", rates);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error fetching rates with filter");
                return OperationResult<IEnumerable<Rate>>.Failure("Error fetching filtered rates.");
            }
        }
        public async Task<OperationResult<Rate>> GetByIdAsync(int id)
        {
            try
            {

                var rate = await _context.Rate
                    .Where(r => r.RateId == id && !r.IsDeleted)  // Obtener por ID, si no está eliminada
                    .FirstOrDefaultAsync();

                if (rate == null)
                    return OperationResult<Rate>.Failure("Rate not found.");

                return OperationResult<Rate>.Success("Rate found.", rate);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error fetching rate by ID");
                return OperationResult<Rate>.Failure("Error fetching rate.");
            }
        }
        public async Task<OperationResult<IEnumerable<Rate>>> GetRatesByCategoryAsync(int categoryId)
        {
            try
            {
                var rates = await _context.Rate
                    .Where(r => r.CategoryId == categoryId && !r.IsDeleted) // Obtener todas las tasas de una categoría y que no estén eliminadas
                    .ToListAsync();

                return OperationResult<IEnumerable<Rate>>.Success("Rates by category retrieved successfully.", rates);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error fetching rates by category");
                return OperationResult<IEnumerable<Rate>>.Failure("Error fetching category rates.");
            }
        }
        public async Task<OperationResult<Rate>> UpdateAsync(Rate entity)
        {
            try
            {
                if (entity == null)
                    return OperationResult<Rate>.Failure("Rate is null.");

                var existing = await _context.Rate.FindAsync(entity.RateId);

                // Validaciones 

                if (existing == null || existing.IsDeleted)
                    return OperationResult<Rate>.Failure("Rate not found or already deleted.");

                existing.CategoryId = entity.CategoryId;
                existing.SeasonId = entity.SeasonId;
                existing.NightPrice = entity.NightPrice;
                existing.UpdatedAt = DateTime.Now.Date;
                existing.UpdatedBy = entity.UpdatedBy;

                _context.Rate.Update(existing);
                await _context.SaveChangesAsync();

                return OperationResult<Rate>.Success("Rate updated successfully.", existing);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error updating rate");
                return OperationResult<Rate>.Failure("Error occurred while updating rate.");
            }
        }
    }
}
