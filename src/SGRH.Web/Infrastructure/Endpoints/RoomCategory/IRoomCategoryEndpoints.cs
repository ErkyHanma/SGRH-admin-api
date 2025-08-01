namespace SGRH.Web.Infrastructure.Endpoints.RoomCategory
{
    public interface IRoomCategoryEndpoints
    {
        string GetAllRoomCategories { get; }
        string GetRoomCategoryById { get; }
        string CreateRoomCategory { get; }
        string ModifyRoomCategory { get; }
        string DisableRoomCategory { get; }
    }
}