using SGRH.Application.Dtos.ServiceModule;
using SGRH.Persistence.Test.Test.ServiceModule.EntityBuilder;

namespace SGRH.Application.Test.Test.ServiceModule.EntityBuilder
{
    public class ServiceDtoBuilder : BaseServiceDtoBuilder<ServiceDto>
    {
        public ServiceDtoBuilder()
        {
            _dto = new ServiceDto();
        }

        public ServiceDtoBuilder WithServiceId(int serviceId)
        {
            _dto.ServiceId = serviceId;
            return this;
        }

        public override ServiceDto Build()
        {
            return _dto;
        }

        public override ServiceDtoBuilder WithTestValues()
        {
            {
                _dto.ServiceId = 1;
                base.WithTestValues();

                return this;
            }
        }
    }
}
