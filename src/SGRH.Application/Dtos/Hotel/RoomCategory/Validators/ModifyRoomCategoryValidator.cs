using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.RoomCategory.Validators
{
    public class ModifyRoomCategoryValidator : BaseRoomCategoryValidator<ModifyRoomCategoryDto>
    {
        public ModifyRoomCategoryValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");
            RuleFor(x => x.UpdatedBy)
                .GreaterThan(0).WithMessage("UpdatedBy must be greater than zero.");
        }

        protected override string GetName(ModifyRoomCategoryDto dto) => dto.Name;
        protected override string GetDescription(ModifyRoomCategoryDto dto) => dto.Description;
        protected override int GetMaxCapacity(ModifyRoomCategoryDto dto) => dto.MaxCapacity;
        protected override string GetAmenities(ModifyRoomCategoryDto dto) => dto.Amenities;
    }
}