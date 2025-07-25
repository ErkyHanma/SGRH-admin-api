namespace SGRH.Web.Models.ReservationModule.Reservation.Response
{
    public class GetAllReservationResponse : BaseResponse<ReservationModel>
    {
        public List<ReservationModel> data { get; set; }
    }
}
