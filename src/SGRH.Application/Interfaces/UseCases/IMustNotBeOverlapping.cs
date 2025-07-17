using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.UseCases
{
    public interface IMustNotBeOverlapping<T>
    {
        Task<OperationResult<string>> Validate(T id1, T id2);
        Task<OperationResult<string>> ValidateWithExclusion(T id1, T id2, T id3);
    }

}
