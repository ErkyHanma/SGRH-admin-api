using SGRH.Application.Dtos.ServiceModule;
using SGRH.Domain.Entities.ServiceModule;


namespace SGRH.Application.Extensions.ServiceModule
{
    public static class ServiceExtension
    {
        public static Service ToDomainEntity(ServiceDto dto)
        {
            return new Service
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
            };
        }

        public static ServiceDto ToDto(Service entity)
        {
            return new ServiceDto
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
            };
        }
    }
}
