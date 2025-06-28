using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Rate
{
    public record CreateRateDto
    {
        public int CategoryId { get; set; }
        public int SeasonId { get; set; }
        public decimal NightPrice { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
