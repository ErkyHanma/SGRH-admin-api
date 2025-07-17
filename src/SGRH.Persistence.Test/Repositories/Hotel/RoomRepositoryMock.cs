using FluentValidation;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** Este es un mock del RoomRepository original es una implementación basica 
  Tiene una logica simplificada que simula el almacenamiento y validaciones **/

namespace SGRH.Persistence.Test.Repositories.Hotel
{
    public class RoomRepositoryMock : IRoomRepository
    {
        private readonly List<RoomDto> _database = new();
        private readonly IValidator<CreateRoomDto> _createValidator;
        private readonly IValidator<DisableRoomDto> _disableValidator;
        private readonly IValidator<ModifyRoomDto> _modifyValidator;
        public RoomRepositoryMock(IValidator<CreateRoomDto> createValidator, 
                                  IValidator<DisableRoomDto> disableValidator,
                                  IValidator<ModifyRoomDto> modifyValidator)
        {
            _createValidator = createValidator;
            _disableValidator = disableValidator;
            _modifyValidator = modifyValidator;
        }
        public async Task<OperationResult<CreateRoomDto>> AddAsync(CreateRoomDto createRoomDto)
        {
            var result = _createValidator.Validate(createRoomDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<CreateRoomDto>.Failure(message);
            }

            var room = new RoomDto
            {
                RoomId = _database.Count + 1,
                RoomNumber = createRoomDto.RoomNumber,
                CategoryId = createRoomDto.CategoryId,
                FloorId = createRoomDto.FloorId,
                Status = createRoomDto.Status,
                CreatedAt = DateTime.Now,
                CreatedBy = createRoomDto.CreatedBy
            };

            _database.Add(room);
            return OperationResult<CreateRoomDto>.Success("Room created.", createRoomDto);

        }

        public async Task<OperationResult<DisableRoomDto>> DeleteAsync(DisableRoomDto disableRoomDto)
        {
            var result = _disableValidator.Validate(disableRoomDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<DisableRoomDto>.Failure(message);
            }

            var room = _database.FirstOrDefault(r => r.RoomId == disableRoomDto.RoomId);  
            
            if (room == null)
                return OperationResult<DisableRoomDto>.Failure("Room not found.");

            // No se marca como eliminado aquí porque en el repositorio original lo hace el SP.

            return OperationResult<DisableRoomDto>.Success("Room deleted.", disableRoomDto);
        }

        public async Task<OperationResult<IEnumerable<RoomDto>>> GetAllAsync()
        {
            return OperationResult<IEnumerable<RoomDto>>.Success("Rooms retireved sucessfully.", _database);
        }

        public async Task<OperationResult<RoomDto>> GetByIdAsync(int id)
        {
            var room = _database.FirstOrDefault(r => r.RoomId ==  id);

            if (room == null)
                return OperationResult<RoomDto>.Failure("Room not found");

            return OperationResult<RoomDto>.Success("Room found.", room);
        }

        public async Task<OperationResult<ModifyRoomDto>> UpdateAsync(ModifyRoomDto modifyRoomDto)
        {
            var result = _modifyValidator.Validate(modifyRoomDto);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                return OperationResult<ModifyRoomDto>.Failure(message);
            }

            var room = _database.FirstOrDefault(r => r.RoomId == modifyRoomDto.RoomId);

            if (room == null)
                return OperationResult<ModifyRoomDto>.Failure("Room not found");

            room.RoomNumber = modifyRoomDto.RoomNumber;
            room.CategoryId = modifyRoomDto.CategoryId;
            room.FloorId = modifyRoomDto.FloorId;
            room.Description = modifyRoomDto.Description;
            room.RoomImgUrl = modifyRoomDto.RoomImgUrl;
            room.Status = modifyRoomDto.Status;

            return OperationResult<ModifyRoomDto>.Success("Room updated successfully.", modifyRoomDto);

        }
    }
}
