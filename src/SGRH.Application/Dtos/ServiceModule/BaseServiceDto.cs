namespace SGRH.Application.Dtos.ServiceModule
{
    public abstract class BaseServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
