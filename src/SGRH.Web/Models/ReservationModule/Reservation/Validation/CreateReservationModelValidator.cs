using SGRH.Web.Models.ReservationModule.Reservation.Response;

namespace SGRH.Web.Models.ReservationModule.Reservation.Validation
{
    public class CreateReservationModelValidator : BaseReservationModelValidator<CreateReservationModel>
    {
        public override CreateReservationResponse Validate(CreateReservationModel model)
        {
            var validationResponse = base.Validate(model);

            if (!validationResponse.isSuccess)
            {
                return new CreateReservationResponse
                {
                    isSuccess = validationResponse.isSuccess,
                    message = validationResponse.message

                };
            }

            return new CreateReservationResponse
            {
                isSuccess = true,
            };
        }
    }
}
