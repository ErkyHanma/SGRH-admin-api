using Microsoft.EntityFrameworkCore;
using SGRH.Domain.Entities.ServiceModule;

namespace SGRH.Persistence.Context.EntityConfigurations
{
    public class ServiceConfiguration
    {
        public static void OnServiceModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("services", "servicesmodule");

                entity.HasKey(e => e.ServiceId);

                entity.Property(e => e.ServiceId)
                      .HasColumnName("service_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("timestamp without time zone"); ;


                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasColumnType("timestamp without time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.DeletedAt)
                      .HasColumnName("deleted_at")
                      .HasColumnType("timestamp without time zone");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            });
        }
    }
}
