namespace SGRH.Application.Dtos.Hotel.RoomCategory
{
    public record class CreateRoomCategoryDto : BaseRoomCategoryDto
    {
        public int CreatedBy { get; set; }
    }
}