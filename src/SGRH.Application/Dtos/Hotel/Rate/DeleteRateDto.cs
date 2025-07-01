using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Rate
{
    public record DeleteRateDto
    {
        public int RateId { get; set; }
        public int DeletedBy { get; set; }

    }
}
