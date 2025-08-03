using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Models.ServiceModule.Validation
{
    public class DeleteServiceModelValidator
    {
        public DeleteServiceResponse Validate(DeleteServiceModel model)
        {
            if (model == null)
            {
                return new DeleteServiceResponse
                {
                    isSuccess = false,
                    message = "Invalid data provided"
                };
            }

            if (model.serviceId < 0)
            {
                return new DeleteServiceResponse
                {
                    isSuccess = false,
                    message = "Invalid ID provided"
                };

            }


            return new DeleteServiceResponse { isSuccess = true };


        }
    }
}
