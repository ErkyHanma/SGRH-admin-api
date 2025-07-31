namespace SGRH.Web.Models.Hotel.Room
{
    public class CreateRoomModel : BaseRoomModel
    {
        public CreateRoomModel()
        {
            status = "available"; // default value
        }
        // Este modelo hereda todo de BaseRoomModel
        // Solo necesita las propiedades que ya están en la base
    }
}
