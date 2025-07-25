namespace SGRH.Web.Models.ReservationModule
{
    public abstract class BaseReservationModel
    {
        public int clientId { get; set; }
        public int roomId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string status { get; set; }
        public int guestCount { get; set; }
        public decimal paymentAmount { get; set; }
    }
}
