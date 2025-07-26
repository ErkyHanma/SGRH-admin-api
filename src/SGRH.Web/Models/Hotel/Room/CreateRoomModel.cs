namespace SGRH.Web.Models.Hotel.Room
{
    public class CreateRoomModel
    {
        public string roomNumber { get; set; }
        public int categoryId { get; set; }
        public int floorId { get; set; }
        public string description { get; set; }
        public string roomImgUrl { get; set; }
        public string status { get; set; } = "available"; // default
        public int createdBy { get; set; }
    }
}
