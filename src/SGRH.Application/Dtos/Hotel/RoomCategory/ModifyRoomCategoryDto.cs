namespace SGRH.Application.Dtos.Hotel.RoomCategory
{
    public record class ModifyRoomCategoryDto : BaseRoomCategoryDto
    {
        public int CategoryId { get; set; }
        public int UpdatedBy { get; set; }
    }
}