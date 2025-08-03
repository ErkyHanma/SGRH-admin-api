using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Models.ServiceModule.Validation
{
    public class EditServiceModelValidator : BaseServiceValidator<ServiceModel>
    {
        public override EditServiceResponse Validate(ServiceModel model)
        {
            var validationResult = base.Validate(model);

            if (validationResult != null)
            {
                return new EditServiceResponse { isSuccess = validationResult.isSuccess, message = validationResult.message };
            }

            return new EditServiceResponse { isSuccess = true };
        }
    }
}
