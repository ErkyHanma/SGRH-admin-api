using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Rate
{
    public record RateDto
    {
        public int RateId { get; set; }
        public int CategoryId { get; set; }
        public int SeasonId { get; set; }
        public decimal NightPrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
    }
}
