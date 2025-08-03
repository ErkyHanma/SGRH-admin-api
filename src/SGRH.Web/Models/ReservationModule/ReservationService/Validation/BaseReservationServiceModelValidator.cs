namespace SGRH.Web.Models.ReservationModule.ReservationService.Validation
{
    public abstract class BaseReservationServiceModelValidator<TModel> where TModel : BaseReservationServiceModel
    {
        public virtual BaseResponse<TModel> Validate(TModel model)
        {
            if (model == null)
                return new BaseResponse<TModel> { isSuccess = false, message = "Model cannot be null." };

            if (model.reservationId <= 0)
                return new BaseResponse<TModel> { isSuccess = false, message = "ReservationId must be greater than zero." };

            if (model.serviceId <= 0)
                return new BaseResponse<TModel> { isSuccess = false, message = "ServiceId must be greater than zero." };

            return new BaseResponse<TModel> { isSuccess = true };
        }
    }
}
