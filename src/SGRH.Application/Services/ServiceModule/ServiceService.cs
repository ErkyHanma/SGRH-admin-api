

using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.ServiceModule;
using SGRH.Application.Dtos.ServiceModule.Validators;
using SGRH.Application.Interfaces.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Application.Interfaces.Services.Service_Module;
using SGRH.Domain.Base;

namespace SGRH.Application.Services.ServiceModule
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IAppLogger<ServiceService> _logger;
        private readonly IServiceMapper _mapper;
        public ServiceService(IServiceRepository serviceRepository, IAppLogger<ServiceService> logger, IServiceMapper mapper
            )
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<ServiceDto>>> GetAllServicesAsync()
        {
            try
            {
                var result = await _serviceRepository.GetAllAsync();


                if (!result.IsSuccess)
                {
                    return OperationResult<IEnumerable<ServiceDto>>.Failure(result.Message);
                }

                if (result.Data is null)
                {
                    return OperationResult<IEnumerable<ServiceDto>>.Failure("No services found.");
                }

                var serviceDtos = result.Data.Select(item => _mapper.ToDto(item));
                return OperationResult<IEnumerable<ServiceDto>>.Success(result.Message, serviceDtos);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while getting the services");
                return OperationResult<IEnumerable<ServiceDto>>.Failure($"An unexpected error occurred while trying to get the services: {ex.Message}");
            }
        }
        public async Task<OperationResult<ServiceDto>> GetServiceByIdAsync(int id)
        {
            try
            {
                var result = await _serviceRepository.GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    return OperationResult<ServiceDto>.Failure(result.Message);
                }

                if (result.Data is null)
                {
                    return OperationResult<ServiceDto>.Failure("No service found with the given ID.");
                }

                var serviceDto = _mapper.ToDto(result.Data);
                return OperationResult<ServiceDto>.Success(result.Message, serviceDto);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while getting the service");
                return OperationResult<ServiceDto>.Failure($"An unexpected error occurred while trying to get the service: {ex.Message}");
            }
        }
        public async Task<OperationResult<CreateServiceDto>> CreateServicesAsync(CreateServiceDto createServiceDto)
        {

            try
            {
                _logger.Info("Creating service", createServiceDto);

                var createDtoValidator = new CreateServiceDtoValidator();
                var validationResult = createDtoValidator.Validate(createServiceDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                // Check if service with the same name already exists
                if (await _serviceRepository.ExistsAsync(nt => nt.Name == createServiceDto.Name))
                {
                    return OperationResult<CreateServiceDto>.Failure($"Service with name {createServiceDto.Name} already exists.");
                }

                var creationResult = await _serviceRepository.AddAsync(_mapper.ToDomainEntityAdd(createServiceDto));

                if (!creationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while creating Service: {creationResult.Message}.");
                    return OperationResult<CreateServiceDto>.Failure($"Error trying to create a Service {creationResult.Message}");
                }

                if (creationResult.Data is null)
                {
                    return OperationResult<CreateServiceDto>.Failure("No services found.");
                }

                return OperationResult<CreateServiceDto>.Success(creationResult.Message, _mapper.ToCreateDto(creationResult.Data));


            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error");
                return OperationResult<CreateServiceDto>.Failure($"Error creating a service {ex.Message}");
            }


        }
        public async Task<OperationResult<ServiceDto>> UpdateServicesAsync(ServiceDto serviceDto)
        {
            try
            {
                _logger.Info($"Updating {serviceDto.Name}");

                var serviceDtoValidator = new ServiceDtoValidator();
                var validationResult = serviceDtoValidator.Validate(serviceDto);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                // Check if service exists
                if (!await _serviceRepository.ExistsAsync(nt => nt.ServiceId == serviceDto.ServiceId))
                {
                    return OperationResult<ServiceDto>.Failure($"Service with ID: {serviceDto.ServiceId} does not exists.");
                }

                var result = await _serviceRepository.UpdateAsync(_mapper.ToDomainEntity(serviceDto));


                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while updating Service: {result.Data} {result.Message}.");
                }


                if (result.Data is null)
                {
                    return OperationResult<ServiceDto>.Failure($"No services found. {result.Message}");
                }



                return OperationResult<ServiceDto>.Success(result.Message, _mapper.ToDto(result.Data));


            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while updating the service");
                return OperationResult<ServiceDto>.Failure($"An unexpected error occurred while trying to update the service: {ex.Message}");
            }
        }
        public async Task<OperationResult<DeleteServiceDto>> DeleteServicesAsync(DeleteServiceDto deleteServiceDto)
        {
            try
            {
                _logger.Info($"Deleting service with Id: {deleteServiceDto.ServiceId}");

                // Check if service exists
                if (!await _serviceRepository.ExistsAsync(nt => nt.ServiceId == deleteServiceDto.ServiceId))
                {
                    return OperationResult<DeleteServiceDto>.Failure($"Service with ID: {deleteServiceDto.ServiceId} does not exists.");
                }


                var result = await _serviceRepository.DeleteAsync(_mapper.ToDomainEntityDelete(deleteServiceDto));

                if (!result.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while deleting Service: {result.Message}.");
                    return OperationResult<DeleteServiceDto>.Failure(result.Message);
                }

                if (result.Data is null)
                {
                    return OperationResult<DeleteServiceDto>.Failure($"No services found. {result.Message}");
                }

                return OperationResult<DeleteServiceDto>.Success(result.Message);

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Unexpected error while deleting the service");
                return OperationResult<DeleteServiceDto>.Failure($"An unexpected error occurred while trying to delete the service: {ex.Message}");
            }
        }


    }


}


