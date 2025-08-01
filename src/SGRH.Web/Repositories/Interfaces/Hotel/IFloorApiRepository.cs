using SGRH.Web.Models.Hotel.Floor;
using SGRH.Web.Models.Hotel.Floor.Responses;

namespace SGRH.Web.Repositories.Interfaces.Hotel
{
    public interface IFloorApiRepository
    {
        Task<GetAllFloorsResponse> GetFloorsAsync();

        Task<GetFloorResponse> GetFloorByIdAsync(int id);

        Task<FloorCreateResponse> CreateFloorAsync(CreateFloorModel model);

        Task<FloorEditResponse> EditFloorAsync(EditFloorModel model);

        Task<DeleteFloorResponse> DeleteFloorAsync(DeleteFloorModel model);
    }
}