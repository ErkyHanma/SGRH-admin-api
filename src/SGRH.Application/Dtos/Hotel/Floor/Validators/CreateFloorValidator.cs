using FluentValidation;

namespace SGRH.Application.Dtos.Hotel.Floor.Validators
{
    public class CreateFloorValidator : BaseFloorValidator<CreateFloorDto>
    {
        public CreateFloorValidator()
        {
            RuleFor(x => x.CreatedBy)
                .GreaterThan(0).WithMessage("CreatedBy must be greater than zero.");
        }

        protected override int GetFloorNumber(CreateFloorDto dto) => dto.FloorNumber;
        protected override string GetStatus(CreateFloorDto dto) => dto.Status;
        protected override string? GetDescription(CreateFloorDto dto) => dto.Description;
    }
}