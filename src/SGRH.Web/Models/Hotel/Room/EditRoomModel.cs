namespace SGRH.Web.Models.Hotel.Room
{
    public class EditRoomModel
    {
        public int roomId { get; set; }
        public string roomNumber { get; set; }
        public int categoryId { get; set; }
        public int floorId { get; set; }
        public string description { get; set; }
        public string roomImgUrl { get; set; }
        public string status { get; set; }
        public int updatedBy { get; set; }
    }
}
