namespace SGRH.Application.Dtos.Hotel.Floor
{
    public record class DisableFloorDto
    {
        public int FloorId { get; set; }
        public int UpdatedBy { get; set; }
    }
}