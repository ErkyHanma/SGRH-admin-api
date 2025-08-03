using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Models.ServiceModule.Validation
{
    public class CreateServiceModelValidator : BaseServiceValidator<CreateServiceModel>
    {
        public override CreateServiceResponse Validate(CreateServiceModel model)
        {
            var response = base.Validate(model);

            if (response != null)
            {

                return new CreateServiceResponse
                {
                    isSuccess = response.isSuccess,
                    message = response.message,
                };
            }

            return new CreateServiceResponse
            {
                isSuccess = true
            };
        }
    }
}
