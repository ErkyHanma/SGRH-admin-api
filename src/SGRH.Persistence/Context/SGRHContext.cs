using Microsoft.EntityFrameworkCore;

using SGRH.Domain.Entities.Hotel;

using SGRH.Domain.Entities.ServiceModule;
using SGRH.Persistence.Context.EntityConfiguration;

namespace SGRH.Persistence.Context
{
    public class SGRHContext : DbContext
    {
        public SGRHContext(DbContextOptions<SGRHContext> options) : base(options) { }

        public DbSet<Service> Service { get; set; }
        
        public DbSet<Rate> Rate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RateConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
