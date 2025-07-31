using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SGRH.Web.Infrastructure.Endpoints.Rate;

namespace SGRH.Web.Infrastructure.Endpoints.Room
{
    public class RoomEndpoints : IRoomEndpoints
    {
        public string GetAllRooms { get; }
        public string GetRoomById { get; }
        public string CreateRoom { get; }
        public string ModifyRoom { get; }
        public string DisableRoom { get; }

        public RoomEndpoints()
        {
            GetAllRooms = "Room/GetRooms";
            GetRoomById = "Room/GetRoomById";
            CreateRoom =  "Room/CreateRoom";
            ModifyRoom =  "Room/ModifyRoom";
            DisableRoom = "Room/DisableRoom";
        }
    }
}
