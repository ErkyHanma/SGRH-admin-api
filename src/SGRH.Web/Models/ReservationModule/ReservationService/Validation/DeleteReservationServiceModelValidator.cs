using SGRH.Web.Models.ReservationModule.ReservationService.Response;

namespace SGRH.Web.Models.ReservationModule.ReservationService.Validation
{
    public class DeleteReservationServiceModelValidator : BaseReservationServiceModelValidator<DeleteReservationServiceModel>
    {
        public override DeleteReservationServiceResponse Validate(DeleteReservationServiceModel model)
        {
            var validationResponse = base.Validate(model);

            if (!validationResponse.isSuccess)
            {
                return new DeleteReservationServiceResponse
                {
                    isSuccess = validationResponse.isSuccess,
                    message = validationResponse.message,
                };
            }

            return new DeleteReservationServiceResponse { isSuccess = true };
        }
    }
}
