using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.ServiceModule;
using SGRH.Persistence.Context;
using System.Linq.Expressions;


namespace SGRH.Persistence.Repositories.Service_Module
{
    public class ServiceRepository : IServiceRepository
    {

        private readonly SGRHContext _context;
        private readonly ILogger<ServiceRepository> _logger;

        public ServiceRepository(SGRHContext context, ILogger<ServiceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<Service>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all Services entities");
                var services = await _context.Service.ToListAsync();

                if (!services.Any())
                {
                    return OperationResult<IEnumerable<Service>>.Success("No services were found.", services);
                }

                return OperationResult<IEnumerable<Service>>.Success($"Found {services.Count} service(s) matching the filter.", services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving service entities.");
                return OperationResult<IEnumerable<Service>>.Failure("An error occurred while retrieving the service entities.");
            }
        }
        public async Task<OperationResult<Service>> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving Service entity with ID: {id}");

                var existingService = await _context.Service.FindAsync(id);

                if (existingService is null)
                {
                    return OperationResult<Service>.Failure("Service was not found with the given ID");
                }

                return OperationResult<Service>.Success($"Service with the ID: {id} found", existingService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving a service entity by ID");
                return OperationResult<Service>.Failure("An error occurred while retrieving the service entity");
            }
        }
        public async Task<OperationResult<Service>> AddAsync(Service entity)
        {
            try
            {
                _logger.LogInformation($"Adding Service entity {entity}");


                if (entity == null)
                {
                    _logger.LogError("Attempted to add a null Service entity ");
                    return OperationResult<Service>.Failure("The Service entity cannot be null");
                }

                await _context.Service.AddAsync(entity);
                await _context.SaveChangesAsync();

                return OperationResult<Service>.Success("Service Entity added succesfully", entity);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a service");
                return OperationResult<Service>.Failure("An error occurred while adding a service");
            }
        }
        public async Task<OperationResult<Service>> UpdateAsync(Service entity)
        {
            try
            {
                _logger.LogInformation($"Updating Service {entity.Name}");


                if (entity == null)
                {
                    _logger.LogError("Attempted to add a null Service entity ");
                    return OperationResult<Service>.Failure("The Service entity cannot be null");
                }

                // Validations:

                var ExistingService = await _context.Service.FindAsync(entity.ServiceId);

                if (ExistingService is null)
                {
                    return OperationResult<Service>.Failure("Service entity not found");
                }

                ExistingService.Name = entity.Name;
                ExistingService.Description = entity.Description;
                ExistingService.Price = entity.Price;
                ExistingService.UpdatedAt = DateTime.Now;
                ExistingService.UpdatedBy = 1; // For now a hard code value
                _context.Service.Update(ExistingService);
                await _context.SaveChangesAsync();

                return OperationResult<Service>.Success("Service Entity update succesfully", ExistingService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating a service entity");
                return OperationResult<Service>.Failure("An error occurred while updating a service entity");
            }
        }
        public async Task<OperationResult<Service>> DeleteAsync(Service entity)
        {
            try
            {
                _logger.LogInformation($"Deleting Service with ID {entity?.ServiceId} and Name '{entity?.Name ?? "N/A"}'");

                if (entity is null)
                {
                    _logger.LogError("Attempted to delete a null Service entity");
                    return OperationResult<Service>.Failure("The Service entity cannot be null");
                }

                var ExistingService = await _context.Service.FindAsync(entity.ServiceId);

                if (ExistingService is null)
                {
                    return OperationResult<Service>.Failure("Service entity not found");
                }

                ExistingService.IsDeleted = true;
                ExistingService.IsActive = false;
                ExistingService.DeleteAt = DateTime.Now;
                ExistingService.DeletedBy = 1; // For now a hardcoded value

                _context.Service.Update(ExistingService);
                await _context.SaveChangesAsync();

                return OperationResult<Service>.Success("Service deleted successfully", ExistingService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Service entity");
                return OperationResult<Service>.Failure("An error occurred while trying to delete the Service entity");
            }
        }
        public async Task<OperationResult<IEnumerable<Service>>> GetAllAsync(Expression<Func<Service, bool>> filter)
        {
            try
            {
                _logger.LogInformation($"Retrieving all Service entities where filter is {filter}");

                var services = await _context.Service.Where(filter).ToListAsync();

                return OperationResult<IEnumerable<Service>>.Success($"Found {services.Count} service(s) matching the filter.", services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving service entities.");
                return OperationResult<IEnumerable<Service>>.Failure("An error occurred while retrieving the service entities.");
            }
        }
        public async Task<bool> ExistsAsync(Expression<Func<Service, bool>> filter)
        {
            return await _context.Service.AnyAsync(filter);
        }
    }
}


