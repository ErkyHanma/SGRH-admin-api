namespace SGRH.Application.Dtos.Hotel.Floor
{
    public record class ModifyFloorDto : BaseFloorDto
    {
        public int FloorId { get; set; }
        public int UpdatedBy { get; set; }
    }
}