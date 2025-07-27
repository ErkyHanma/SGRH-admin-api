namespace SGRH.Web.Models.Hotel.Floor
{
    public class FloorModel
    {
        public int FloorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int FloorNumber { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
   
}
