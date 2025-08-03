namespace SGRH.Web.Models.ServiceModule.Validation
{
    public abstract class BaseServiceValidator<TModel> where TModel : BaseServiceModel
    {
        public virtual BaseResponse<TModel> Validate(TModel model)
        {
            if (model == null)
            {
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "Service entity cannot be null."
                };
            }

            if (string.IsNullOrWhiteSpace(model.name))
            {
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "Service name is required."
                };
            }


            if (string.IsNullOrWhiteSpace(model.description))
            {
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "Service description is required."
                };
            }

            if (model.price < 0)
            {
                return new BaseResponse<TModel>
                {
                    isSuccess = false,
                    message = "Service price cannot be negative."
                };
            }



            return new BaseResponse<TModel>
            {
                isSuccess = true
            };
        }
    }
}



