using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGRH.Domain.Base;

namespace SGRH.Application.Interfaces.UseCases
{
    public interface IMustBeUniqueValidator<TValue>
    {
        Task<OperationResult<string>> ValidateCreate(TValue value);
        Task<OperationResult<string>> ValidateModify(int id, TValue value);
    }
}