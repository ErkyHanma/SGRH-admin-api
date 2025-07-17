using FluentValidation;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGRH.Persistence.Test.Repositories.Hotel
{
    public class RoomCategoryRepositoryMock : IRoomCategoryRepository
    {
        private readonly List<RoomCategoryDto> _database = new();
        private readonly IValidator<CreateRoomCategoryDto> _createValidator;
        private readonly IValidator<DisableRoomCategoryDto> _disableValidator;
        private readonly IValidator<ModifyRoomCategoryDto> _modifyValidator;

        public RoomCategoryRepositoryMock(IValidator<CreateRoomCategoryDto> createValidator,
                                          IValidator<DisableRoomCategoryDto> disableValidator,
                                          IValidator<ModifyRoomCategoryDto> modifyValidator)
        {
            _createValidator = createValidator;
            _disableValidator = disableValidator;
            _modifyValidator = modifyValidator;

            // Pre-llenar la base de datos simulada
            _database.Add(new RoomCategoryDto { CategoryId = 1, Name = "Standard Single", Description = "Basic single room", MaxCapacity = 1, Amenities = "Wifi", CreatedAt = DateTime.Now, CreatedBy = 1 });
            _database.Add(new RoomCategoryDto { CategoryId = 2, Name = "Standard Double", Description = "Basic double room", MaxCapacity = 2, Amenities = "Wifi", CreatedAt = DateTime.Now, CreatedBy = 1 });
        }

        public async Task<OperationResult<CreateRoomCategoryDto>> AddAsync(CreateRoomCategoryDto createRoomCategoryDto)
        {
            var result = _createValidator.Validate(createRoomCategoryDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<CreateRoomCategoryDto>.Failure(message);
            }

            if (_database.Any(rc => rc.Name != null && rc.Name.Equals(createRoomCategoryDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return OperationResult<CreateRoomCategoryDto>.Failure($"Room category name '{createRoomCategoryDto.Name}' already exists.");
            }

            var newCategory = new RoomCategoryDto
            {
                CategoryId = _database.Count > 0 ? _database.Max(rc => rc.CategoryId) + 1 : 1,
                Name = createRoomCategoryDto.Name,
                Description = createRoomCategoryDto.Description,
                MaxCapacity = createRoomCategoryDto.MaxCapacity,
                Amenities = createRoomCategoryDto.Amenities,
                CreatedAt = DateTime.Now,
                CreatedBy = createRoomCategoryDto.CreatedBy
            };

            _database.Add(newCategory);
            return OperationResult<CreateRoomCategoryDto>.Success("Room category created.", createRoomCategoryDto);
        }

        public async Task<OperationResult<DisableRoomCategoryDto>> DeleteAsync(DisableRoomCategoryDto disableRoomCategoryDto)
        {
            var result = _disableValidator.Validate(disableRoomCategoryDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<DisableRoomCategoryDto>.Failure(message);
            }

            var category = _database.FirstOrDefault(rc => rc.CategoryId == disableRoomCategoryDto.CategoryId);

            if (category == null)
                return OperationResult<DisableRoomCategoryDto>.Failure("Room category not found.");

            return OperationResult<DisableRoomCategoryDto>.Success("Room category deleted.", disableRoomCategoryDto);
        }

        public async Task<OperationResult<IEnumerable<RoomCategoryDto>>> GetAllAsync()
        {
            // Solo retorna las categorías activas 
            return OperationResult<IEnumerable<RoomCategoryDto>>.Success("Room categories retrieved successfully.", _database.ToList());
        }

        public async Task<OperationResult<RoomCategoryDto>> GetByIdAsync(int id)
        {
            var category = _database.FirstOrDefault(rc => rc.CategoryId == id);

            if (category == null)
                return OperationResult<RoomCategoryDto>.Failure("Room category not found.");

            return OperationResult<RoomCategoryDto>.Success("Room category found.", category);
        }

        public async Task<OperationResult<ModifyRoomCategoryDto>> UpdateAsync(ModifyRoomCategoryDto modifyRoomCategoryDto)
        {
            var result = _modifyValidator.Validate(modifyRoomCategoryDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<ModifyRoomCategoryDto>.Failure(message);
            }

            var category = _database.FirstOrDefault(rc => rc.CategoryId == modifyRoomCategoryDto.CategoryId);

            if (category == null)
                return OperationResult<ModifyRoomCategoryDto>.Failure("Room category not found.");

            if (_database.Any(rc => rc.Name != null && rc.Name.Equals(modifyRoomCategoryDto.Name, StringComparison.OrdinalIgnoreCase) && rc.CategoryId != modifyRoomCategoryDto.CategoryId))
            {
                return OperationResult<ModifyRoomCategoryDto>.Failure($"Room category name '{modifyRoomCategoryDto.Name}' already exists for another category.");
            }

            category.Name = modifyRoomCategoryDto.Name;
            category.Description = modifyRoomCategoryDto.Description;
            category.MaxCapacity = modifyRoomCategoryDto.MaxCapacity;
            category.Amenities = modifyRoomCategoryDto.Amenities;

            return OperationResult<ModifyRoomCategoryDto>.Success("Room category updated successfully.", modifyRoomCategoryDto);
        }
    }
}