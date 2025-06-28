using SGRH.Domain.Base;
using SGRH.Domain.Entities.ServiceModule;

namespace SGRH.Application.Dtos.ServiceModule.Validator
{
    public class ServiceValidator
    {
        public static OperationResult<Service> Validate(Service service)
        {

            if (service == null)
                return OperationResult<Service>.Failure("Service entity cannot be null.");

            if (string.IsNullOrWhiteSpace(service.Name))
                return OperationResult<Service>.Failure("Service name is required.");

            if (string.IsNullOrWhiteSpace(service.Description))
                return OperationResult<Service>.Failure("Service description is required.");

            if (service.Price < 0)
                return OperationResult<Service>.Failure("Service price cannot be negative.");

            return OperationResult<Service>.Success("Service validated successfully.", service);
        }
    }
}
