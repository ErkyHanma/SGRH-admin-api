using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.UseCases.Hotel.Room
{
    public class RoomCategoryMustExist
    {
        private readonly IRoomCategoryRepository _roomCategoryRepository;

        public RoomCategoryMustExist(IRoomCategoryRepository roomCategoryRepository)
        {
            _roomCategoryRepository = roomCategoryRepository;
        }

        public async Task<OperationResult<string>> Validate(int categoryId)
        {
            var result = await _roomCategoryRepository.GetByIdAsync(categoryId);
            if (!result.IsSuccess || result.Data == null)
               return OperationResult<string>.Failure($"CategoryId {categoryId} does not exist.");

            return OperationResult<string>.Success("Category exists.");

        }
    }
}
