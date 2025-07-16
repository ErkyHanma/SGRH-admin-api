using SGRH.Application.Dtos.ServiceModule;

namespace SGRH.Application.Test.Test.ServiceModule.EntityBuilder
{
    public class DeleteServiceDtoBuilder
    {

        private DeleteServiceDto _dto = new();

        public DeleteServiceDtoBuilder WithServiceId(int serviceId)
        {
            _dto.ServiceId = serviceId;
            return this;
        }

        public DeleteServiceDto Build()
        {
            return _dto;
        }

        public DeleteServiceDtoBuilder WithTestValues()
        {
            {
                _dto.ServiceId = 1;

                return this;
            }
        }
    }
}
