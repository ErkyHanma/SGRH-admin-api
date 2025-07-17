using SGRH.Domain.Base;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.UseCases
{
    public interface IMustNotBeOccupied<T>
    {
        OperationResult<string> Validate(T entity);
    }
}
