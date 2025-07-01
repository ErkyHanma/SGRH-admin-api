using SGRH.Application.Dtos.ServiceModule;
using SGRH.Domain.Entities.ServiceModule;

namespace SGRH.Application.Interfaces.Mappers.ServiceModule
{
    public interface IServiceMapper
    {
        Service ToDomainEntity(ServiceDto dto);
        Service ToDomainEntityAdd(CreateServiceDto dto);
        Service ToDomainEntityDelete(DeleteServiceDto deleteServiceDto);
        ServiceDto ToDto(Service entity);
        CreateServiceDto ToCreateDto(Service entity);
    }
}





