using SGRH.Web.Models.ReservationModule.Reservation.Response;

namespace SGRH.Web.Models.ReservationModule.Reservation.Validation
{
    public class DeleteReservationModelValidator
    {
        public DeleteReservationResponse Validate(DeleteReservationModel model)
        {

            if (model == null)
            {
                return new DeleteReservationResponse
                {
                    isSuccess = false,
                    message = "The data provided is invalid."
                };


            }

            if (model.reservationId < 0)
            {
                return new DeleteReservationResponse
                {
                    isSuccess = false,
                    message = "The ID provided is invalid."
                };
            }


            return new DeleteReservationResponse
            {
                isSuccess = true,
            };

        }
    }
}
