using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Rate
{
    public abstract record BaseRateDto
    {
        public int CategoryId { get; set; }
        public int SeasonId { get; set; }
        public decimal NightPrice { get; set; }

    }
}
