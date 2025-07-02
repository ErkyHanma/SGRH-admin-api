namespace SGRH.Application.Dtos.Hotel.RoomCategory
{
    public record class DisableRoomCategoryDto
    {
        public int CategoryId { get; set; }
        public int UpdatedBy { get; set; }
   
    }
}