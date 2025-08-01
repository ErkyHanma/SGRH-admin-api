namespace SGRH.Web.Models.Hotel.Floor
{
    public class FloorModel: BaseFloorModel
    {
        public int FloorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

    }
}