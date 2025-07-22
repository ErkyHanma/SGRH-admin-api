using Microsoft.EntityFrameworkCore;
using SGRH.Application.Dtos.ServiceModule.Validator;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Domain.Base;
using SGRH.Domain.Entities.ServiceModule;
using SGRH.Persistence.Test.Context;
using System.Linq.Expressions;

namespace SGRH.Persistence.Test.Repositories.ServiceModule
{
    public class ServiceRepositoryMock : IServiceRepository
    {
        private readonly SGRHContextInMemory _context;

        public ServiceRepositoryMock(SGRHContextInMemory context)
        {
            _context = context;
        }

        public async Task<OperationResult<IEnumerable<Service>>> GetAllAsync()
        {
            try
            {
                var services = await _context.Service
                                    .Where(s => !s.IsDeleted && s.IsActive)
                                    .OrderByDescending(s => s.UpdatedAt)
                                    .ToListAsync();



                if (!services.Any())
                {
                    return OperationResult<IEnumerable<Service>>.Success("No services were found.", services);
                }

                return OperationResult<IEnumerable<Service>>.Success($"Found {services.Count} service(s) matching the filter.", services);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Service>>.Failure($"An error occurred while retrieving the service entities. {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<Service>.Failure("Invalid service ID");
                }


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
                return OperationResult<Service>.Failure($"An error occurred while retrieving the service entity {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> AddAsync(Service entity)
        {
            try
            {
                var validationResult = ServiceValidator.Validate(entity);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                await _context.Service.AddAsync(entity);
                await _context.SaveChangesAsync();


                return OperationResult<Service>.Success("Service entity added successfully.", entity);
            }
            catch (Exception ex)
            {
                return OperationResult<Service>.Failure($"An error occurred while adding a service: {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> UpdateAsync(Service entity)
        {
            try
            {
                var validationResult = ServiceValidator.Validate(entity);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

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
                ExistingService.UpdatedBy = entity.UpdatedBy;
                ExistingService.UpdatedAt = DateTime.Now;


                _context.Service.Update(ExistingService);
                await _context.SaveChangesAsync();

                return OperationResult<Service>.Success("Service Entity update successfully", ExistingService);
            }
            catch (Exception ex)
            {
                return OperationResult<Service>.Failure($"An error occurred while updating a service entity {ex.Message}");
            }
        }
        public async Task<OperationResult<Service>> DeleteAsync(Service entity)
        {
            try
            {
                var ExistingService = await _context.Service
                                            .FirstOrDefaultAsync(s => s.ServiceId == entity.ServiceId && !s.IsDeleted && s.IsActive);

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

                return OperationResult<Service>.Success($"Service {ExistingService.Name} deleted successfully", ExistingService);
            }
            catch (Exception ex)
            {
                return OperationResult<Service>.Failure($"An error occurred while trying to delete the Service entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Service>>> GetAllAsync(Expression<Func<Service, bool>> filter)
        {
            if (filter == null)
            {
                return OperationResult<IEnumerable<Service>>.Failure("Filter expression cannot be null.");
            }

            try
            {

                var services = await _context.Service.Where(filter).ToListAsync();

                return OperationResult<IEnumerable<Service>>.Success($"Found {services.Count} service(s) matching the filter.", services);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Service>>.Failure($"An error occurred while retrieving the service entities. Error:{ex.Message}");
            }
        }
        public async Task<bool> ExistsAsync(Expression<Func<Service, bool>> filter)
        {

            if (filter == null)
            {
                return false;
            }

            return await _context.Service.AnyAsync(filter);

        }
    }
}
