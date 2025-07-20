using SGRH.Application.Dtos.ReservationModule.Reservation;
using SGRH.Persistence.Test.Test.ReservationModule.Reservation.EntityBuilder;

public class CreateReservationDtoBuilder : BaseReservationDtoBuilder<CreateReservationDto>
{
    public CreateReservationDtoBuilder()
    {
        _entity = new CreateReservationDto();
    }

    public override CreateReservationDto Build()
    {
        return _entity;
    }

    public override CreateReservationDtoBuilder WithTestValues()
    {
        _entity.ClientId = 3;
        _entity.RoomId = 3;
        _entity.StartDate = new DateTime(2055, 7, 10);
        _entity.EndDate = new DateTime(2055, 7, 11);
        _entity.Status = "Pending";
        _entity.GuestCount = 2;
        _entity.PaymentAmount = 100;

        return this;
    }
}
