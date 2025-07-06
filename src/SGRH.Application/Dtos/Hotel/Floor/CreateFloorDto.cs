namespace SGRH.Application.Dtos.Hotel.Floor
{
    public record class CreateFloorDto : BaseFloorDto
    {
        public int CreatedBy { get; set; }
    }
}