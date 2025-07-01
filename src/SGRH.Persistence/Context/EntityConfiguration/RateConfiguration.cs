using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGRH.Domain.Entities.Hotel;

namespace SGRH.Persistence.Context.EntityConfiguration
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> entity)
        {
            entity.ToTable("rate", "hotel");

            entity.HasKey(e => e.RateId);

            entity.Property(e => e.RateId)
                  .HasColumnName("rate_id")
                  .ValueGeneratedOnAdd();

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.SeasonId).HasColumnName("season_id");
            entity.Property(e => e.NightPrice).HasColumnName("night_price");

            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.CreatedAt).HasColumnName("created_at")
                .HasColumnType("timestamp without time zone"); ;

            entity.Property(e => e.CreatedBy).HasColumnName("created_by");

            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at")
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.Property(e => e.DeleteAt).HasColumnName("deleted_at")
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        }
    }
}
