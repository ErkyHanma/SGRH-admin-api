using Microsoft.Extensions.Configuration;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Base;
using SGRH.Infrastructure.Common.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Application.Services.Hotel
{
    public sealed class RoomCategoryService : IRoomCategoryService
    {
        private readonly IRoomCategoryRepository _roomCategoryRepository;
        private readonly IAppLogger<RoomCategoryService> _logger;
        private readonly IConfiguration _configuration; // Although not used in RoomService logic, it's injected. Keeping for consistency.

        public RoomCategoryService(IRoomCategoryRepository roomCategoryRepository, IAppLogger<RoomCategoryService> logger, IConfiguration configuration)
        {
            _roomCategoryRepository = roomCategoryRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<CreateRoomCategoryDto>> CreateRoomCategory(CreateRoomCategoryDto createRoomCategoryDto)
        {
            OperationResult<CreateRoomCategoryDto> operationResult = new OperationResult<CreateRoomCategoryDto>();
            try
            {
                _logger.Info("Creating room category", createRoomCategoryDto);

                if (createRoomCategoryDto is null)
                {
                    operationResult = OperationResult<CreateRoomCategoryDto>.Failure("Object CreateRoomCategoryDto is required.");
                    return operationResult;
                }

                operationResult = await _roomCategoryRepository.AddAsync(createRoomCategoryDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred while creating: {operationResult.Message} Persisting room category.");
                    return operationResult;
                }

                _logger.Info($"The room category {createRoomCategoryDto.Name} was created successfully.", createRoomCategoryDto);
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error creating a room category");
                operationResult = OperationResult<CreateRoomCategoryDto>.Failure($"Error creating a room category: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<DisableRoomCategoryDto>> DeleteRoomCategory(DisableRoomCategoryDto disableRoomCategoryDto)
        {
            OperationResult<DisableRoomCategoryDto> operationResult = new OperationResult<DisableRoomCategoryDto>();
            try
            {
                _logger.Info("Disabling room category ID {CategoryId}", disableRoomCategoryDto.CategoryId);

                if (disableRoomCategoryDto is null)
                {
                    operationResult = OperationResult<DisableRoomCategoryDto>.Failure("Object DisableRoomCategoryDto is required.");
                    return operationResult;
                }

                operationResult = await _roomCategoryRepository.DeleteAsync(disableRoomCategoryDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred while disabling room category: {operationResult.Message} Persisting room category.");
                    return operationResult;
                }

                _logger.Info($"The room category ID {disableRoomCategoryDto.CategoryId} was disabled successfully.");
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error disabling a room category");
                operationResult = OperationResult<DisableRoomCategoryDto>.Failure($"Error trying to disable a room category: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<IEnumerable<RoomCategoryDto>>> GetRoomCategories()
        {
            OperationResult<IEnumerable<RoomCategoryDto>> operationResult = new OperationResult<IEnumerable<RoomCategoryDto>>();
            try
            {
                _logger.Info("Retrieving all room categories");
                operationResult = await _roomCategoryRepository.GetAllAsync();

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred on _roomCategoryRepository.GetAllAsync() while retrieving room categories: {operationResult.Message}");
                }

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving room categories");
                operationResult = OperationResult<IEnumerable<RoomCategoryDto>>.Failure($"Error retrieving room categories: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<RoomCategoryDto>> GetRoomCategoryById(int categoryId)
        {
            OperationResult<RoomCategoryDto> operationResult = new OperationResult<RoomCategoryDto>();
            try
            {
                _logger.Info("Retrieving room category by ID {CategoryId}", categoryId);
                operationResult = await _roomCategoryRepository.GetByIdAsync(categoryId);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred on _roomCategoryRepository.GetByIdAsync({categoryId}) while retrieving a room category: {operationResult.Message}");
                }
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving a room category by ID");
                operationResult = OperationResult<RoomCategoryDto>.Failure($"Error retrieving a room category: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<ModifyRoomCategoryDto>> UpdateRoomCategory(ModifyRoomCategoryDto modifyRoomCategoryDto)
        {
            OperationResult<ModifyRoomCategoryDto> operationResult = new OperationResult<ModifyRoomCategoryDto>();
            try
            {
                _logger.Info("Updating room category ID {CategoryId}", modifyRoomCategoryDto.CategoryId);

                if (modifyRoomCategoryDto is null)
                {
                    operationResult = OperationResult<ModifyRoomCategoryDto>.Failure("Object ModifyRoomCategoryDto is required.");
                    return operationResult;
                }

                operationResult = await _roomCategoryRepository.UpdateAsync(modifyRoomCategoryDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred while updating room category: {operationResult.Message} Persisting room category.");
                    return operationResult;
                }

                _logger.Info($"The room category {modifyRoomCategoryDto.Name} was updated successfully.");
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error during UpdateRoomCategory");
                operationResult = OperationResult<ModifyRoomCategoryDto>.Failure($"Error trying to update a room category: {ex.Message}");
                return operationResult;
            }
        }
    }
}