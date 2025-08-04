using Microsoft.EntityFrameworkCore;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Common.Common;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.ServiceModule;
using SGRH.Persistence.Context;
using System.Linq.Expressions;


namespace SGRH.Persistence.Repositories.Service_Module
{
    public class ServiceRepository : IServiceRepository
    {

        private readonly SGRHContext _context;
        private readonly IAppLogger<ServiceRepository> _logger;

        public ServiceRepository(SGRHContext context, IAppLogger<ServiceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<Service>>> GetAllAsync()
        {
            try
            {
                _logger.Info("Retrieving all Services entities");
                var services = await _context.Service
                                    .Where(s => !s.IsDeleted && s.IsActive)
                                    .OrderByDescending(s => s.UpdatedAt)
                                    .ToListAsync();



                if (!services.Any())
                {
                    return OperationResult<IEnumerable<Service>>.Success("No services were found.", services);
                }

                return OperationResult<IEnumerable<Service>>.Success($"Services retrieve successfully", services);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error while retrieving service entities.");
                return OperationResult<IEnumerable<Service>>.Failure($"An error occurred while retrieving the service entities. {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.ErrorNoEx($"Tried to find Service with invalid ID: {id}.");
                    return OperationResult<Service>.Failure("Invalid service ID");
                }

                _logger.Info($"Retrieving Service entity with ID: {id}");

                var existingService = await _context.Service
                                            .FirstOrDefaultAsync(s => s.ServiceId == id && !s.IsDeleted && s.IsActive);


                if (existingService is null)
                {
                    return OperationResult<Service>.Failure("Service was not found with the given ID");
                }

                return OperationResult<Service>.Success($"Service with the ID: {id} found", existingService);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error while retrieving a service entity by ID");
                return OperationResult<Service>.Failure($"An error occurred while retrieving the service entity {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> AddAsync(Service entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.ErrorNoEx("Tried to add null Service entity.");
                    return OperationResult<Service>.Failure("Service entity cannot be null.");
                }

                _logger.Info($"Adding Service entity with Name: {entity.Name}");

                entity.CreatedBy = 1; // User session ID (For now Hardcode value)
                await _context.Service.AddAsync(entity);
                await _context.SaveChangesAsync();


                return OperationResult<Service>.Success("Service entity added successfully.", entity);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error while adding a service");
                return OperationResult<Service>.Failure($"An error occurred while adding a service: {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> UpdateAsync(Service entity)
        {
            try
            {
                _logger.Info($"Updating Service entity with Name: {entity.Name}");


                var ExistingService = await _context.Service
                                            .FirstOrDefaultAsync(s => s.ServiceId == entity.ServiceId && !s.IsDeleted && s.IsActive);

                if (ExistingService is null)
                {
                    return OperationResult<Service>.Failure("Service entity not found");
                }

                // Actualizar los campos relevantes
                ExistingService.Name = entity.Name;
                ExistingService.Description = entity.Description;
                ExistingService.Price = entity.Price;
                ExistingService.IsActive = entity.IsActive;
                ExistingService.UpdatedBy = 1; // User session ID (For now Hardcode value)
                ExistingService.UpdatedAt = DateTime.Now;

                _context.Service.Update(ExistingService);
                await _context.SaveChangesAsync();

                return OperationResult<Service>.Success("Service Entity update successfully", ExistingService);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error while updating a service entity");
                return OperationResult<Service>.Failure($"An error occurred while updating a service entity {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> DeleteAsync(Service entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.ErrorNoEx("Tried to add null Service entity.");
                    return OperationResult<Service>.Failure("Service entity cannot be null.");
                }

                _logger.Info($"Deleting Service entity");

                var ExistingService = await _context.Service
                                            .FirstOrDefaultAsync(s => s.ServiceId == entity.ServiceId && !s.IsDeleted && s.IsActive);

                if (ExistingService is null)
                {
                    return OperationResult<Service>.Failure("Service entity not found");
                }

                ExistingService.IsDeleted = true;
                ExistingService.IsActive = false;
                ExistingService.DeleteAt = DateTime.Now;
                ExistingService.DeletedBy = 1; // User session ID (For now Hardcode value)

                _context.Service.Update(ExistingService);
                await _context.SaveChangesAsync();

                return OperationResult<Service>.Success($"Service {ExistingService.Name} deleted successfully", ExistingService);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error while deleting Service entity");
                return OperationResult<Service>.Failure($"An error occurred while trying to delete the Service entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Service>>> GetAllAsync(Expression<Func<Service, bool>> filter)
        {
            if (filter == null)
            {
                _logger.ErrorNoEx("Tried to retrieve services with a null filter.");
                return OperationResult<IEnumerable<Service>>.Failure("Filter expression cannot be null.");
            }

            try
            {
                _logger.Info($"Retrieving all Service entities with provided filter.");

                var services = await _context.Service.Where(filter).ToListAsync();

                return OperationResult<IEnumerable<Service>>.Success($"Found {services.Count} service(s) matching the filter.", services);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error while retrieving service entities.");
                return OperationResult<IEnumerable<Service>>.Failure($"An error occurred while retrieving the service entities. Error:{ex.Message}");
            }
        }
        public async Task<bool> ExistsAsync(Expression<Func<Service, bool>> filter)
        {

            if (filter == null)
            {
                _logger.ErrorNoEx("Tried to check existence of services with a null filter.");
                return false;
            }

            return await _context.Service.AnyAsync(filter);

        }
    }
}


