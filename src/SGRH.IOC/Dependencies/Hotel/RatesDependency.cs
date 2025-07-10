using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Dtos.Hotel.Rate.Validators;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Interfaces.Mappers.Hotel;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Services.Hotel;
using SGRH.Persistence.Repositories.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGRH.Application.UseCases.Hotel.Rate;

namespace SGRH.IOC.Dependencies.Hotel
{
    public static class RatesDependency
    {
        public static void AddRatesDependency(this IServiceCollection service)
        {
            service.AddScoped<IRatesRepository, RatesRepository>();
            service.AddTransient<IRatesService, RatesService>();
            service.AddScoped<IRateMapper, RateMapper>();

            //Use cases

            service.AddScoped<RatesMustNotBeOverlapping>();

            //Fluent Validation

            service.AddScoped<IValidator<CreateRateDto>, CreateRateValidator>();
            service.AddScoped<IValidator<UpdateRateDto>, UpdateRateValidator>();
            service.AddScoped<IValidator<DeleteRateDto>, DeleteRateValidator>();
        } 
    }
}
