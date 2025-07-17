using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.UseCases
{
    public interface IReportDateMustBeCorrect<T>
    {
        OperationResult<string> Validate(T data);
    }
}
