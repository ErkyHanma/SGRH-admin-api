using Microsoft.EntityFrameworkCore;
using SGRH.Domain.Entities.ReservationModule;
using SGRH.Domain.Entities.ServiceModule;
using SGRH.Persistence.Context.EntityConfiguration;

namespace SGRH.Persistence.Test.Context
{
    public class SGRHContextInMemory : DbContext
    {
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Service> Service { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("SGRHDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Service
            ServiceConfiguration.OnServiceModelCreating(modelBuilder);

            // Rate
            //modelBuilder.ApplyConfiguration(new RateConfiguration());

            base.OnModelCreating(modelBuilder);
        }



    }
}
