using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGRH.Domain.Base;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.UseCases
{
    
    public interface IMustNotHaveAssociationsValidator<TId>
    {
        Task<OperationResult<string>> Validate(TId id);
    }
}