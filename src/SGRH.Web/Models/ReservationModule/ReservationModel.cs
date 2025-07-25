namespace SGRH.Web.Models.ReservationModule
{
    public class ReservationModel : BaseReservationModel
    {
        public int reservationId { get; set; }
        public DateTime reservationDate { get; set; }
        public int servicesCount { get; set; }
        public decimal totalServicesCost { get; set; }
        public string serviceNames { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}


