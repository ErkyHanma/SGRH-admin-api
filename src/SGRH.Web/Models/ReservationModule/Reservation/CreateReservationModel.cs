namespace SGRH.Web.Models.ReservationModule.Reservation
{
    public class CreateReservationModel
    {
        public int clientId { get; set; }
        public int roomId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string status { get; set; }
        public int guestCount { get; set; }
        public int paymentAmount { get; set; }
    }
}
