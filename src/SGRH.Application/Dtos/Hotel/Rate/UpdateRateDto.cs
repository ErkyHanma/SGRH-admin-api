using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Rate
{
    public record UpdateRateDto : BaseRateDto
    {
        public int RateId { get; set; } 
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
