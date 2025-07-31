namespace SGRH.Web.Models.Hotel.Rates
{
    public abstract class BaseRateModel
    {
        // Propiedades de negocio
        public int categoryId { get; set; }
        public int seasonId { get; set; }
        public decimal nightPrice { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public DateTime createdAt { get; set; }
        public int createdBy { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}
