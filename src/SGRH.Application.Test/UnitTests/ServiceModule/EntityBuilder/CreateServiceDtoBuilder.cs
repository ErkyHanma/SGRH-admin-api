using SGRH.Application.Dtos.ServiceModule;
using SGRH.Persistence.Test.Test.ServiceModule.EntityBuilder;

namespace SGRH.Application.Test.Test.ServiceModule.EntityBuilder
{
    public class CreateServiceDtoBuilder : BaseServiceDtoBuilder<CreateServiceDto>
    {
        public CreateServiceDtoBuilder()
        {
            _dto = new CreateServiceDto();
        }


        public override CreateServiceDto Build()
        {
            return _dto;
        }

    }
}
