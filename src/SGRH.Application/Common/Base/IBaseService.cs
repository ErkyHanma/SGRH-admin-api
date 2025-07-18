using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGRH.Application.Dtos.UserManagement.Client;

namespace SGRH.Application.Common.Base
{
    public interface IBaseService<TEntityDto, TCreateDto, TUpdateDto, TRemoveDto>
      where TEntityDto : DtoBase
      where TCreateDto : class
      where TUpdateDto : DtoBase
      where TRemoveDto : DtoBase
    {
        Task<IEnumerable<TEntityDto>> GetAll();
        Task<TEntityDto> GetById(int id);
        Task<int> Create(TCreateDto dto);
        Task<bool> Update(TUpdateDto dto);
        Task<bool> Remove(TRemoveDto dto);
    }
}
