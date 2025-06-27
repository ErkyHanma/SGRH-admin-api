using Microsoft.EntityFrameworkCore;

using SGRH.Domain.Entities.Hotel;

using SGRH.Domain.Entities.ServiceModule;
using SGRH.Persistence.Context.EntityConfigurations;

namespace SGRH.Persistence.Context
{
    public class SGRHContext : DbContext
    {
        public SGRHContext(DbContextOptions<SGRHContext> options) : base(options) { }

        public DbSet<Service> Service { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ServiceConfiguration.OnServiceModelCreating(modelBuilder);
        }


        public DbSet<Rate> Rate { get; set; }
    }
}
