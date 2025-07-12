using SGRH.Domain.Entities.ServiceModule;

namespace SGRH.Persistence.Test.Test.ServiceModule.EntityBuilder
{
    // Follows The Unit Test Data Builder Pattern to create instances of the Service entity for testing purposes.
    public class ServiceEntityBuilder
    {
        public const int TEST_SERVICE_ID = 2;
        private Service _entity = new();

        public ServiceEntityBuilder WithId(int serviceID)
        {
            _entity.ServiceId = serviceID;
            return this;
        }

        public ServiceEntityBuilder WithName(string name)
        {
            _entity.Name = name;
            return this;
        }

        public ServiceEntityBuilder WithDescription(string description)
        {
            _entity.Description = description;
            return this;
        }

        public ServiceEntityBuilder WithPrice(decimal price)
        {
            _entity.Price = price;
            return this;
        }

        public Service Build()
        {
            return _entity;
        }

        public ServiceEntityBuilder WithTestValues()
        {
            _entity = new Service
            {
                ServiceId = TEST_SERVICE_ID,
                Name = "Test Service new try it",
                Description = "This is a test service description.",
                Price = 99.99m,
            };

            return this;
        }
    }
}
