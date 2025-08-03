using SGRH.Web.Models.ReservationModule.ReservationService.Response;

namespace SGRH.Web.Models.ReservationModule.ReservationService.Validation
{
    public class CreateReservationServiceModelValidator : BaseReservationServiceModelValidator<AddReservationServiceModel>
    {
        public override AddReservationServiceResponse Validate(AddReservationServiceModel model)
        {
            var validationResponse = base.Validate(model);

            if (!validationResponse.isSuccess)
            {
                return new AddReservationServiceResponse
                {
                    isSuccess = validationResponse.isSuccess,
                    message = validationResponse.message,
                };
            }

            return new AddReservationServiceResponse { isSuccess = true };
        }
    }
}
