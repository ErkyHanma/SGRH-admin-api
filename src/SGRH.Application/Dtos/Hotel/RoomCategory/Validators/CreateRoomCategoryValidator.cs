using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.RoomCategory.Validators
{
    public class CreateRoomCategoryValidator : BaseRoomCategoryValidator<CreateRoomCategoryDto>
    {
        public CreateRoomCategoryValidator()
        {
            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }

        protected override string GetName(CreateRoomCategoryDto dto) => dto.Name;
        protected override string GetDescription(CreateRoomCategoryDto dto) => dto.Description;
        protected override int GetMaxCapacity(CreateRoomCategoryDto dto) => dto.MaxCapacity;
        protected override string GetAmenities(CreateRoomCategoryDto dto) => dto.Amenities;
    }
}