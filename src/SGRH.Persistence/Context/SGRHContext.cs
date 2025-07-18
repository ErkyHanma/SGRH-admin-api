using Microsoft.EntityFrameworkCore;
using SGRH.Domain.Entities.Hotel;
using SGRH.Domain.Entities.ServiceModule;
using SGRH.Domain.Entities.UserManagement;
using SGRH.Persistence.Context.EntityConfiguration;
using SGRH.Persistence.Context.EntityConfigurations;

namespace SGRH.Persistence.Context
{
    public class SGRHContext : DbContext
    {
        public SGRHContext(DbContextOptions<SGRHContext> options) : base(options) { }

        public DbSet<Service> Service { get; set; }
        public DbSet<Rate> Rate { get; set; }
        // public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ServiceConfiguration.OnServiceModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RateConfiguration());
            // modelBuilder.ApplyConfiguration(new ClientConfiguration());
        }
    }
}