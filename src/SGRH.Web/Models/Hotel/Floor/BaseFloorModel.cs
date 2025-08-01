namespace SGRH.Web.Models.Hotel.Floor
{
    public abstract class BaseFloorModel
    {
        public int FloorNumber { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}