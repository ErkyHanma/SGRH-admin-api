namespace SGRH.Web.Infrastructure.Endpoints.Rate
{
    public interface IRoomEndpoints
    {
        string GetAllRooms { get; }
        string GetRoomById { get; }
        string CreateRoom { get; }
        string ModifyRoom { get; }
        string DisableRoom { get; }
    }
}
