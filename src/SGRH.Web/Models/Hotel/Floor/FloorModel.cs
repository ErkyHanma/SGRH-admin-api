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

    public class GetAllFloorResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public List<FloorModel> data { get; set; }
    }
    public class GetFloorResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public FloorModel data { get; set; }
    }

}
