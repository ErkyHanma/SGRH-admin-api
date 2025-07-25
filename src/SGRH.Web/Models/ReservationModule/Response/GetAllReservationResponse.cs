namespace SGRH.Web.Models.ReservationModule.Response
{
    public class GetAllReservationResponse : BaseResponse<ReservationModel>
    {
        public List<ReservationModel> data { get; set; }
    }
}
