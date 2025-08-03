using Microsoft.Extensions.Configuration;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Domain.Base;
using SGRH.Infrastructure.Common.Logging;

namespace SGRH.Application.Services.Hotel
{
    public sealed class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IAppLogger<RoomService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMustNotBeOccupied<RoomDto> _roomMustNotBeOccupied;
        private readonly IMustExistValidator<int> _roomCategoryMustExist;
        private readonly IMustExistValidator<int> _roomFloorMustExist;

        public RoomService(
            IRoomRepository roomRepository,
            IAppLogger<RoomService> logger,
            IConfiguration configuration,
            IMustNotBeOccupied<RoomDto> roomMustNotBeOccupied,
            IMustExistValidator<int> roomCategoryMustExist,
            IMustExistValidator<int> roomFloorMustExist)
        {
            _roomRepository = roomRepository;
            _logger = logger;
            _configuration = configuration;
            _roomMustNotBeOccupied = roomMustNotBeOccupied;
            _roomCategoryMustExist = roomCategoryMustExist;
            _roomFloorMustExist = roomFloorMustExist;
        }

        public async Task<OperationResult<CreateRoomDto>> CreateRoom(CreateRoomDto createRoomDto)
        {
            try
            {
                _logger.Info("Creating room", createRoomDto);

                // Validaciones

                if (createRoomDto is null)
                    return OperationResult<CreateRoomDto>.Failure("CreateRoomDto is required.");

                var categoryValidation = await _roomCategoryMustExist.Validate(createRoomDto.CategoryId);
                if (!categoryValidation.IsSuccess)
                    return OperationResult<CreateRoomDto>.Failure(categoryValidation.Message);

                var floorValidation = await _roomFloorMustExist.Validate(createRoomDto.FloorId); 
                if (!floorValidation.IsSuccess)
                    return OperationResult<CreateRoomDto>.Failure(floorValidation.Message);

                var operationResult = await _roomRepository.AddAsync(createRoomDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error occurred while creating: {operationResult.Message}");
                    return operationResult;
                }

                _logger.Info($"Room {createRoomDto} created successfully.");
                return operationResult;

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error");
                return OperationResult<CreateRoomDto>.Failure($"Error creating room: {ex.Message}");
            }
        }

        public async Task<OperationResult<DisableRoomDto>> DeleteRoom(DisableRoomDto disableRoomDto)
        {
            try
            {
                _logger.Info("Deleting room", disableRoomDto);

                // Validaciones

                if (disableRoomDto is null)
                    return OperationResult<DisableRoomDto>.Failure("DisableRoomDto is required.");

                var roomResult = await _roomRepository.GetByIdAsync(disableRoomDto.RoomId);

                if (!roomResult.IsSuccess || roomResult.Data == null)
                    return OperationResult<DisableRoomDto>.Failure($"RoomId {disableRoomDto.RoomId} does not exist.");

                if (!_roomMustNotBeOccupied.Validate(roomResult.Data).IsSuccess)
                    return OperationResult<DisableRoomDto>.Failure("The room is occupied and cannot be modified.");

                var operationResult = await _roomRepository.DeleteAsync(disableRoomDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error occurred while deleting room: {operationResult.Message}");
                    return operationResult;
                }

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error");
                return OperationResult<DisableRoomDto>.Failure($"Error deleting room: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<RoomDto>>> GetRooms()
        {
            try
            {
                var operationResult = await _roomRepository.GetAllAsync();

                if (!operationResult.IsSuccess)
                    _logger.ErrorNoEx($"Error retrieving rooms: {operationResult.Message}");

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving rooms");
                return OperationResult<IEnumerable<RoomDto>>.Failure($"Error retrieving rooms: {ex.Message}");
            }
        }

        public async Task<OperationResult<RoomDto>> GetRoomsById(int roomId)
        {
            try
            {
                var operationResult = await _roomRepository.GetByIdAsync(roomId);

                if (!operationResult.IsSuccess)
                    _logger.ErrorNoEx($"Error retrieving room: {operationResult.Message}");

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving room");
                return OperationResult<RoomDto>.Failure($"Error retrieving room: {ex.Message}");
            }
        }

        public async Task<OperationResult<ModifyRoomDto>> UpdateRoom(ModifyRoomDto modifyRoomDto)
        {
            try
            {
                _logger.Info("Updating room", modifyRoomDto);

                // Validaciones

                if (modifyRoomDto is null)
                    return OperationResult<ModifyRoomDto>.Failure("ModifyRoomDto is required.");

                var categoryValidation = await _roomCategoryMustExist.Validate(modifyRoomDto.CategoryId);
                if (!categoryValidation.IsSuccess)
                    return OperationResult<ModifyRoomDto>.Failure(categoryValidation.Message);

                var floorValidation = await _roomFloorMustExist.Validate(modifyRoomDto.FloorId);
                if (!floorValidation.IsSuccess)
                    return OperationResult<ModifyRoomDto>.Failure(floorValidation.Message);

                var roomResult = await _roomRepository.GetByIdAsync(modifyRoomDto.RoomId);
                if (!roomResult.IsSuccess || roomResult.Data == null)
                    return OperationResult<ModifyRoomDto>.Failure($"RoomId {modifyRoomDto.RoomId} does not exist.");

                if (!_roomMustNotBeOccupied.Validate(roomResult.Data).IsSuccess)
                    return OperationResult<ModifyRoomDto>.Failure("The room is occupied and cannot be modified.");

                var operationResult = await _roomRepository.UpdateAsync(modifyRoomDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error occurred while updating room: {operationResult.Message}");
                    return operationResult;
                }

                _logger.Info($"Room {modifyRoomDto.RoomNumber} updated successfully.");
                return operationResult;

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error updating room");
                return OperationResult<ModifyRoomDto>.Failure($"Error updating room: {ex.Message}");
            }
        }
    }
}
