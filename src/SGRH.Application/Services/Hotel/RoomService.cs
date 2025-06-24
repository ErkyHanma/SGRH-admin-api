using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Services.Hotel
{
    public sealed class RoomService : IRoomService // HARDCODED
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IAppLogger<RoomService> _logger;
        private readonly IConfiguration _configuration;
        public RoomService(IRoomRepository roomRepository, IAppLogger<RoomService> logger, IConfiguration configuration)
        {
            _roomRepository = roomRepository;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<OperationResult<CreateRoomDto>> CreateRoom(CreateRoomDto createRoomDto)
        {
            //Estructura basica (Esto es lo que se quiere?)

            OperationResult<CreateRoomDto> operationResult = new OperationResult<CreateRoomDto>();

            try
            {
                _logger.Info("Creating room", createRoomDto);

                if (createRoomDto is null)
                {
                    operationResult = OperationResult<CreateRoomDto>.Failure("Object CreateRoomDto is required.");
                    return operationResult;
                }

                operationResult = await _roomRepository.AddAsync(createRoomDto);

                // Validaciones (agregar mas)

                if (operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while creating: {operationResult.Message} Persisting room.");
                    return operationResult;
                }

                _logger.Info($"The room {createRoomDto} was created successfully.", createRoomDto);

                return operationResult;

            } 
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error"); // deberia estar en el archivo de configuracion?
                operationResult = OperationResult<CreateRoomDto>.Failure($"Error creating a room {ex.Message}");
            }

            return OperationResult<CreateRoomDto>.Success("Success");
        }

        public async Task<OperationResult<DisableRoomDto>> DeleteRoom(DisableRoomDto disableRoomDto)
        {
            OperationResult<DisableRoomDto> operationResult = new OperationResult<DisableRoomDto>();
            try
            {
                ///
                _logger.Info("Deleting a room", disableRoomDto);

                if (disableRoomDto is null)
                {
                    operationResult = OperationResult<DisableRoomDto>.Failure("Object DisableRoomDto is required.");
                    return operationResult;
                }

                operationResult = await _roomRepository.DeleteAsync(disableRoomDto);

                // Validaciones (agregar mas)

                if (operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while deleting a room: {operationResult.Message} Persisting room.");
                    return operationResult;
                }

            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error"); // deberia estar en el archivo de configuracion?
                operationResult = OperationResult<DisableRoomDto>.Failure($"Error trying to update a room {ex.Message}");
            }
            return operationResult;
        }
    
        public async Task<OperationResult<IEnumerable<RoomDto>>> GetRooms()
        {
            OperationResult<IEnumerable<RoomDto>> operationResult = new OperationResult<IEnumerable<RoomDto>>();

            try
            {
                operationResult = await _roomRepository.GetAllAsync();
                
                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured on _roomRepository.GetAllAsync() while retrieving rooms: {operationResult.Message}");
                }

                return operationResult;

            } 
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error"); // deberia estar en el archivo de configuracion?
                operationResult = OperationResult<IEnumerable<RoomDto>>.Failure($"Error retrieving rooms {ex.Message}");
            }
            return operationResult;
        }

        public async Task<OperationResult<RoomDto>> GetRoomsById(int roomId)
        {
            OperationResult<RoomDto> operationResult = new OperationResult<RoomDto>();
            try
            {
                operationResult = await _roomRepository.GetByIdAsync(roomId);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured on _roomRepository.GetByIdAsync(roomId) while retrieving a room: {operationResult.Message}");
                }
                return operationResult;

            } 
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error"); // deberia estar en el archivo de configuracion?
                operationResult = OperationResult<RoomDto>.Failure($"Error retrieving a room {ex.Message}");
            }
            return operationResult;
        }

        public async Task<OperationResult<ModifyRoomDto>> UpdateRoom(ModifyRoomDto modifyRoomDto)
        {
            OperationResult<ModifyRoomDto> operationResult = new OperationResult<ModifyRoomDto>();
            try
            {
                ///
                _logger.Info("Updating a room", modifyRoomDto);

                if (modifyRoomDto is null)
                {
                    operationResult = OperationResult<ModifyRoomDto>.Failure("Object ModifyRoomDto is required.");
                    return operationResult;
                }

                operationResult = await _roomRepository.UpdateAsync(modifyRoomDto);

                // Validaciones (agregar mas)

                if (operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occured while updating a room: {operationResult.Message} Persisting room.");
                    return operationResult;
                }

            } 
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error"); // deberia estar en el archivo de configuracion?
                operationResult = OperationResult<ModifyRoomDto>.Failure($"Error trying to update a room {ex.Message}");
            }
            return operationResult;
        }
    }
}
