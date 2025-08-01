using SGRH.Web.Infrastructure.Endpoints.RoomCategory;

namespace SGRH.Web.Infrastructure.Endpoints.RoomCategory
{
    public class RoomCategoryEndpoints : IRoomCategoryEndpoints
    {
        public string GetAllRoomCategories { get; }
        public string GetRoomCategoryById { get; }
        public string CreateRoomCategory { get; }
        public string ModifyRoomCategory { get; }
        public string DisableRoomCategory { get; }

        public RoomCategoryEndpoints()
        {
            GetAllRoomCategories = "RoomCategory/GetRoomCategories";
            GetRoomCategoryById = "RoomCategory/GetRoomCategoryById";
            CreateRoomCategory = "RoomCategory/CreateRoomCategory";
            ModifyRoomCategory = "RoomCategory/ModifyRoomCategory";
            DisableRoomCategory = "RoomCategory/DisableRoomCategory";
        }
    }
}