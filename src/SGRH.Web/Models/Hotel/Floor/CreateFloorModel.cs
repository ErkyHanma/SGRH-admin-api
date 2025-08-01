namespace SGRH.Web.Models.Hotel.Floor
{
    public class CreateFloorModel
    {
        public int FloorNumber { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
    }
}