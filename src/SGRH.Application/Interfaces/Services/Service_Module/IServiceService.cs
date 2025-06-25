using SGRH.Application.Dtos.ServiceModule;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.Services.Service_Module
{
    public interface IServiceService
    {
        Task<OperationResult<ServiceDto>> GetAllServicesAsync();
        Task<OperationResult<ServiceDto>> GetServiceByIdAsync(int id);
        Task<OperationResult<ServiceDto>> CreateServicesAsync(CreateServiceDto createServiceDto);
        Task<OperationResult<ServiceDto>> UpdateServicesAsync(ServiceDto serviceDto);
        Task<OperationResult<ServiceDto>> DeleteServicesAsync(ServiceDto serviceDto);

    }
}
