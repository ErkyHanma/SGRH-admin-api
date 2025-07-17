using Microsoft.EntityFrameworkCore;
using SGRH.Domain.Entities.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Persistence.Test.Context
{
    public class RateContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("SGRHConnection");
        }
        public DbSet<Rate> Rate { get; set; }
    }
}
