using SGRH.Application.Dtos.ServiceModule;
using SGRH.Application.Interfaces.Mappers.ServiceModule;
using SGRH.Domain.Entities.ServiceModule;

namespace SGRH.Application.Common.Mappers.ServiceModule
{
    public class ServiceMapper : IServiceMapper
    {
        public Service ToDomainEntity(ServiceDto dto)
        {
            return new Service
            {
                ServiceId = dto.ServiceId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
            };
        }

        public Service ToDomainEntityAdd(CreateServiceDto dto)
        {
            return new Service
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
            };
        }

        public Service ToDomainEntityDelete(DeleteServiceDto dto)
        {
            return new Service
            {
                ServiceId = dto.ServiceId,

            };
        }

        public ServiceDto ToDto(Service entity)
        {
            return new ServiceDto
            {
                ServiceId = entity.ServiceId,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
            };
        }
        public CreateServiceDto ToCreateDto(Service entity)
        {
            return new CreateServiceDto
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
            };
        }




    }
}


