using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder;

public class CreateReservationDtoBuilder : BaseReservationDtoBuilder<CreateReservationDto>
{
    public CreateReservationDtoBuilder()
    {
        _dto = new CreateReservationDto();
    }

    public override CreateReservationDto Build()
    {
        return _dto;
    }

    public override CreateReservationDtoBuilder WithTestValues()
    {
        _dto.ClientId = 3;
        _dto.RoomId = 3;
        _dto.StartDate = new DateTime(2055, 7, 10);
        _dto.EndDate = new DateTime(2055, 7, 11);
        _dto.Status = "Pending";
        _dto.GuestCount = 2;
        _dto.PaymentAmount = 100;

        return this;
    }
}
