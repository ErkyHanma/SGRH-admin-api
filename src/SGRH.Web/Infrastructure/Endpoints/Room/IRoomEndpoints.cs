namespace SGRH.Web.Infrastructure.Endpoints.Room
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
