namespace SGRH.Web.Models.ReservationModule.Reservation.Validation
{
    public abstract class BaseReservationModelValidator<TModel> where TModel : BaseReservationModel
    {
        public virtual BaseResponse<TModel> Validate(TModel model)
        {
            if (model == null)
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "Dto cannot be null."
                };


            if (model.clientId <= 0)
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "ClientId must be greater than zero."
                };


            if (model.roomId <= 0)
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "RoomId must be greater than zero."
                };


            if (model.startDate >= model.endDate)
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "StartDate must be before EndDate."
                };


            if (string.IsNullOrWhiteSpace(model.status))
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "Status is required."
                };


            if (model.guestCount <= 0)
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "GuestCount must be greater than zero."
                };


            if (model.paymentAmount < 0)

                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "PaymentAmount cannot be negative."
                };

            return new BaseResponse<TModel>
            {
                isSuccess = true
            };
        }
    }
}

