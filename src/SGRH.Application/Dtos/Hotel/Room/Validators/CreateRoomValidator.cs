using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    public class CreateRoomValidator : BaseRoomValidator<CreateRoomDto>
    {
        public CreateRoomValidator()
        {
            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }

        protected override string GetRoomNumber(CreateRoomDto dto) => dto.RoomNumber;
        protected override string GetStatus(CreateRoomDto dto) => dto.Status;
        protected override int GetCategoryId(CreateRoomDto dto) => dto.CategoryId;
        protected override int GetFloorId(CreateRoomDto dto) => dto.FloorId;
        protected override string? GetDescription(CreateRoomDto dto) => dto.Description;
        protected override string? GetRoomImgUrl(CreateRoomDto dto) => dto.RoomImgUrl;

    }
}
