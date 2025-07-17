using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.UseCases
{
    public interface IMustExistValidator<T>
    {
        Task<OperationResult<string>> Validate(T id);
    }

}
