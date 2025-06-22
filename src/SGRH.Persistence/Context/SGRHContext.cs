using Microsoft.EntityFrameworkCore;

using SGRH.Domain.Entities.Hotel;

using SGRH.Domain.Entities.ServiceModule;

namespace SGRH.Persistence.Context
{
    public class SGRHContext : DbContext
    {
        public SGRHContext(DbContextOptions<SGRHContext> options) : base(options) { }

        public DbSet<Service> Service { get; set; }
        
        public DbSet<Rate> Rate { get; set; }
    }
}
