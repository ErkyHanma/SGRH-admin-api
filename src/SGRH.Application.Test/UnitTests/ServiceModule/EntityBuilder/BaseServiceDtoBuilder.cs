using SGRH.Application.Dtos.ServiceModule;

namespace SGRH.Persistence.Test.Test.ServiceModule.EntityBuilder
{
    // Follows The Unit Test Data Builder Pattern to create instances of the Service entity for testing purposes.
    public abstract class BaseServiceDtoBuilder<T> where T : BaseServiceDto
    {
        protected T _dto;

        public BaseServiceDtoBuilder<T> WithName(string name)
        {
            _dto.Name = name;
            return this;
        }

        public BaseServiceDtoBuilder<T> WithDescription(string description)
        {
            _dto.Description = description;
            return this;
        }

        public BaseServiceDtoBuilder<T> WithPrice(decimal price)
        {
            _dto.Price = price;
            return this;
        }

        public abstract T Build();


        public virtual BaseServiceDtoBuilder<T> WithTestValues()
        {
            {
                _dto.Name = "Test Service new try it";
                _dto.Description = "This is a test service description.";
                _dto.Price = 99.99m;
            }


            return this;
        }
    }
}
