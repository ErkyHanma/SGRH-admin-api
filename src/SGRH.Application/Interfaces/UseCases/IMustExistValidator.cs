using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGRH.Domain.Base;


namespace SGRH.Application.Interfaces.UseCases
{
    public interface IMustExistValidator<TId>
    {
        Task<OperationResult<string>> Validate(TId id);
    }
}