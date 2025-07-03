using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.Hotel.Rate.Validators
{
    public class BaseRateValidator<T> : AbstractValidator<T> where T : BaseRateDto
    {
        public BaseRateValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

            RuleFor(x => x.SeasonId)
                .GreaterThan(0).WithMessage("SeasonId must be greater than zero.");

            RuleFor(x => x.NightPrice)
                .GreaterThan(0).WithMessage("NightPrice must be greater than zero.");
        }
    }
}
