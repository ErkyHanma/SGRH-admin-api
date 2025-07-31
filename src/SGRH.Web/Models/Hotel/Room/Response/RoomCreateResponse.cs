using SGRH.Web.Models.Hotel.Room;

namespace SGRH.Web.Models.Hotel.Room.Responses
{
    public class RoomCreateResponse : BaseResponse
    {
        public CreateRoomModel data { get; set; }
    }
}
