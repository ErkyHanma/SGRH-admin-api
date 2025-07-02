using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Report;
using SGRH.Application.Services.Report;
using SGRH.Persistence.Repositories.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.IOC.Dependencies.Report
{
    public static class ReportDependency
    {
        public static void AddReportDependency(this IServiceCollection service)
        {
           service.AddScoped<IReportRepository, ReportRepository>();
           service.AddTransient<IReportService, ReportService>();
        }
    }
}
