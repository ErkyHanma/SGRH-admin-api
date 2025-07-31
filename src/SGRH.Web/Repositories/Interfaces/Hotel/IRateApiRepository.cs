using SGRH.Web.Models.Hotel.Room.Responses;
using SGRH.Web.Models.Hotel.Room;
using SGRH.Web.Models.Hotel.Rates;
using SGRH.Web.Models.Hotel.Rates.Responses;

namespace SGRH.Web.Repositories.Interfaces.Hotel
{
    public interface IRateApiRepository
    {
        Task<GetAllRatesResponse> GetRatesAsync();

        Task<GetRateResponse> GetRateByIdAsync(int id);

        Task<RateCreateResponse> CreateRateAsync(RateCreateModel model);

        Task<RateEditResponse> EditRateAsync(RateEditModel model);

        Task<DeleteRateResponse> DeleteRateAsync(RateDeleteModel model);
    }
}
