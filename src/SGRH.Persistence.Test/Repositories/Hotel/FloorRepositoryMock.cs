using FluentValidation;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace SGRH.Persistence.Test.Repositories.Hotel
{
    public class FloorRepositoryMock : IFloorRepository
    {
        private readonly List<FloorDto> _database = new();
        private readonly IValidator<CreateFloorDto> _createValidator;
        private readonly IValidator<DisableFloorDto> _disableValidator;
        private readonly IValidator<ModifyFloorDto> _modifyValidator;

        public FloorRepositoryMock(IValidator<CreateFloorDto> createValidator,
                                   IValidator<DisableFloorDto> disableValidator,
                                   IValidator<ModifyFloorDto> modifyValidator)
        {
            _createValidator = createValidator;
            _disableValidator = disableValidator;
            _modifyValidator = modifyValidator;

           
            _database.Add(new FloorDto { FloorId = 1, FloorNumber = 1, Description = "Primer Piso", Status = "active", CreatedAt = DateTime.Now, CreatedBy = 1 });
            _database.Add(new FloorDto { FloorId = 2, FloorNumber = 2, Description = "Segundo Piso", Status = "active", CreatedAt = DateTime.Now, CreatedBy = 1 });
        }

        public async Task<OperationResult<CreateFloorDto>> AddAsync(CreateFloorDto createFloorDto)
        {
            var result = _createValidator.Validate(createFloorDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<CreateFloorDto>.Failure(message);
            }

            // Simular unicidad del número de piso para el mock (la lógica real está en el caso de uso)
            if (_database.Any(f => f.FloorNumber == createFloorDto.FloorNumber))
            {
                return OperationResult<CreateFloorDto>.Failure($"Floor number {createFloorDto.FloorNumber} already exists.");
            }

            var newFloor = new FloorDto
            {
                FloorId = _database.Count > 0 ? _database.Max(f => f.FloorId) + 1 : 1,
                FloorNumber = createFloorDto.FloorNumber,
                Description = createFloorDto.Description,
                Status = createFloorDto.Status,
                CreatedAt = DateTime.Now,
                CreatedBy = createFloorDto.CreatedBy
            };

            _database.Add(newFloor);
            return OperationResult<CreateFloorDto>.Success("Floor created.", createFloorDto);
        }

        public async Task<OperationResult<DisableFloorDto>> DeleteAsync(DisableFloorDto disableFloorDto)
        {
            var result = _disableValidator.Validate(disableFloorDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<DisableFloorDto>.Failure(message);
            }

            var floor = _database.FirstOrDefault(f => f.FloorId == disableFloorDto.FloorId);

            if (floor == null)
                return OperationResult<DisableFloorDto>.Failure("Floor not found.");

            // Simular eliminación lógica
            floor.Status = "inactive"; 

            return OperationResult<DisableFloorDto>.Success("Floor deleted.", disableFloorDto);
        }

        public async Task<OperationResult<IEnumerable<FloorDto>>> GetAllAsync()
        {
            // Solo retorna los activos y no eliminados lógicamente
            var activeFloors = _database.Where(f => f.Status == "active").ToList();
            return OperationResult<IEnumerable<FloorDto>>.Success("Floors retrieved successfully.", activeFloors);
        }

        public async Task<OperationResult<FloorDto>> GetByIdAsync(int id)
        {
            var floor = _database.FirstOrDefault(f => f.FloorId == id && f.Status == "active");

            if (floor == null)
                return OperationResult<FloorDto>.Failure("Floor not found.");

            return OperationResult<FloorDto>.Success("Floor found.", floor);
        }

        public async Task<OperationResult<ModifyFloorDto>> UpdateAsync(ModifyFloorDto modifyFloorDto)
        {
            var result = _modifyValidator.Validate(modifyFloorDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<ModifyFloorDto>.Failure(message);
            }

            var floor = _database.FirstOrDefault(f => f.FloorId == modifyFloorDto.FloorId);

            if (floor == null)
                return OperationResult<ModifyFloorDto>.Failure("Floor not found.");

            // Simular unicidad del número de piso para el mock (la lógica real está en el caso de uso)
            if (_database.Any(f => f.FloorNumber == modifyFloorDto.FloorNumber && f.FloorId != modifyFloorDto.FloorId))
            {
                return OperationResult<ModifyFloorDto>.Failure($"Floor number {modifyFloorDto.FloorNumber} already exists for another floor.");
            }

            floor.FloorNumber = modifyFloorDto.FloorNumber;
            floor.Description = modifyFloorDto.Description;
            floor.Status = modifyFloorDto.Status;

            return OperationResult<ModifyFloorDto>.Success("Floor updated successfully.", modifyFloorDto);
        }
    }
}