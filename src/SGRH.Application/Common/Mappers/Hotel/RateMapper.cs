using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Interfaces.Mappers.Hotel;
using SGRH.Domain.Entities.Hotel;

public class RateMapper : IRateMapper
{
    public Rate MapFromDto(CreateRateDto dto)
    {
        return new Rate
        {
            CategoryId = dto.CategoryId,
            SeasonId = dto.SeasonId,
            NightPrice = dto.NightPrice,
            IsActive = true,
            IsDeleted = false,
            CreatedBy = dto.CreatedBy,
            CreatedAt = dto.CreatedAt != default ? dto.CreatedAt : DateTime.Now
        };
    }

    public CreateRateDto MapToCreatedDto(Rate entity)
    {
        return new CreateRateDto
        {
            CategoryId = entity.CategoryId,
            SeasonId = entity.SeasonId,
            NightPrice = entity.NightPrice,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy
        };
    }

    public void ApplyDeleteDto(Rate entity, DeleteRateDto dto)
    {
        entity.IsDeleted = true;
        entity.DeletedBy = dto.DeletedBy;
    }

    public RateDto MapToDto(Rate entity)
    {
        return new RateDto
        {
            RateId = entity.RateId,
            CategoryId = entity.CategoryId,
            SeasonId = entity.SeasonId,
            NightPrice = entity.NightPrice,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedBy = entity.UpdatedBy,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public void ApplyUpdateDto(Rate entity, UpdateRateDto dto)
    {
        entity.CategoryId = dto.CategoryId;
        entity.SeasonId = dto.SeasonId;
        entity.NightPrice = dto.NightPrice;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedAt = dto.UpdatedAt != default ? dto.UpdatedAt : DateTime.Now;
    }
}

