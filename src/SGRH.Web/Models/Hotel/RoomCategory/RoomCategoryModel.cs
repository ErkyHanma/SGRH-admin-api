namespace SGRH.Web.Models.Hotel.RoomCategory
{
    public class RoomCategoryModel
    {
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxCapacity { get; set; }
        public string Amenities { get; set; }
    }

    public class GetAllRoomCategoryResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public List<RoomCategoryModel> data { get; set; }
    }

    public class GetRoomCategoryResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public RoomCategoryModel data { get; set; }
    }

}
