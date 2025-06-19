using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Room.Validators
{
    public class ModifyRoomValidator : BaseRoomValidator<ModifyRoomDto>
    {
        public ModifyRoomValidator() 
        {
            RuleFor(x => x.RoomId) 
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
            RuleFor(x => x.UpdatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }
        protected override string GetRoomNumber(ModifyRoomDto dto) => dto.RoomNumber;
        protected override string GetStatus(ModifyRoomDto dto) => dto.Status;
        protected override int GetCategoryId(ModifyRoomDto dto) => dto.CategoryId;
        protected override int GetFloorId(ModifyRoomDto dto) => dto.FloorId;
        protected override string? GetDescription(ModifyRoomDto dto) => dto.Description;
        protected override string? GetRoomImgUrl(ModifyRoomDto dto) => dto.RoomImgUrl;

    }
}
