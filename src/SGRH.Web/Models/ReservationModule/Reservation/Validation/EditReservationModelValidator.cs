using SGRH.Web.Models.ReservationModule.Reservation.Response;

namespace SGRH.Web.Models.ReservationModule.Reservation.Validation
{
    public class EditReservationModelValidator : BaseReservationModelValidator<EditReservationModel>
    {
        public override EditReservationResponse Validate(EditReservationModel model)
        {
            var validationResult = base.Validate(model);

            if (!validationResult.isSuccess)
            {
                return new EditReservationResponse
                {
                    isSuccess = validationResult.isSuccess,
                    message = validationResult.message
                };
            }

            if (model.reservationId < 0)
            {
                return new EditReservationResponse
                {
                    isSuccess = false,
                    message = "Invalid ID provided."
                };
            }

            return new EditReservationResponse
            {
                isSuccess = true
            };
        }
    }
}
