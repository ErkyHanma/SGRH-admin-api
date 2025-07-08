using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Base;
using SGRH.Application.UseCases.Hotel.RoomCategory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Application.Services.Hotel
{
    public sealed class RoomCategoryService : IRoomCategoryService
    {
        private readonly IRoomCategoryRepository _roomCategoryRepository;
        private readonly IAppLogger<RoomCategoryService> _logger;
        private readonly IConfiguration _configuration;
        private readonly RoomCategoryNameMustBeUnique _roomCategoryNameMustBeUnique; 
        private readonly RoomCategoryMustNotHaveAssociatedRooms _roomCategoryMustNotHaveAssociatedRooms;

        public RoomCategoryService(
            IRoomCategoryRepository roomCategoryRepository,
            IAppLogger<RoomCategoryService> logger,
            IConfiguration configuration,
            RoomCategoryNameMustBeUnique roomCategoryNameMustBeUnique, 
            RoomCategoryMustNotHaveAssociatedRooms roomCategoryMustNotHaveAssociatedRooms 
        )
        {
            _roomCategoryRepository = roomCategoryRepository;
            _logger = logger;
            _configuration = configuration;
            _roomCategoryNameMustBeUnique = roomCategoryNameMustBeUnique;
            _roomCategoryMustNotHaveAssociatedRooms = roomCategoryMustNotHaveAssociatedRooms;
        }

        public async Task<OperationResult<CreateRoomCategoryDto>> CreateRoomCategory(CreateRoomCategoryDto createRoomCategoryDto)
        {
            try
            {
                _logger.Info("Creating room category", createRoomCategoryDto);

                if (createRoomCategoryDto is null)
                    return OperationResult<CreateRoomCategoryDto>.Failure("CreateRoomCategoryDto is required.");

                //Caso de Uso: RoomCategoryNameMustBeUnique (para creación)
                var uniqueNameValidation = await _roomCategoryNameMustBeUnique.ValidateCreate(createRoomCategoryDto.Name);
                if (!uniqueNameValidation.IsSuccess)
                {
                    _logger.ErrorNoEx($"Validation failed for CreateRoomCategory: {uniqueNameValidation.Message}");
                    return OperationResult<CreateRoomCategoryDto>.Failure(uniqueNameValidation.Message);
                }
                //Fin Caso de Uso

                var operationResult = await _roomCategoryRepository.AddAsync(createRoomCategoryDto);

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
                return OperationResult<CreateRoomCategoryDto>.Failure($"Error creating a room category: {ex.Message}");
            }
        }

        public async Task<OperationResult<DisableRoomCategoryDto>> DeleteRoomCategory(DisableRoomCategoryDto disableRoomCategoryDto)
        {
            try
            {
                _logger.Info("Disabling room category ID {CategoryId}", disableRoomCategoryDto.CategoryId);

                if (disableRoomCategoryDto is null)
                    return OperationResult<DisableRoomCategoryDto>.Failure("DisableRoomCategoryDto is required.");

                //Caso de Uso: RoomCategoryMustNotHaveAssociatedRooms
                var associatedRoomsValidation = await _roomCategoryMustNotHaveAssociatedRooms.Validate(disableRoomCategoryDto.CategoryId);
                if (!associatedRoomsValidation.IsSuccess)
                {
                    _logger.ErrorNoEx($"Validation failed for DeleteRoomCategory: {associatedRoomsValidation.Message}");
                    return OperationResult<DisableRoomCategoryDto>.Failure(associatedRoomsValidation.Message);
                }
                //Fin Caso de Uso

                var operationResult = await _roomCategoryRepository.DeleteAsync(disableRoomCategoryDto);

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
                return OperationResult<DisableRoomCategoryDto>.Failure($"Error trying to disable a room category: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<RoomCategoryDto>>> GetRoomCategories()
        {
            try
            {
                _logger.Info("Retrieving all room categories");
                var operationResult = await _roomCategoryRepository.GetAllAsync();

                if (!operationResult.IsSuccess)
                    _logger.ErrorNoEx($"An error has occurred on _roomCategoryRepository.GetAllAsync() while retrieving room categories: {operationResult.Message}");

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving room categories");
                return OperationResult<IEnumerable<RoomCategoryDto>>.Failure($"Error retrieving room categories: {ex.Message}");
            }
        }

        public async Task<OperationResult<RoomCategoryDto>> GetRoomCategoryById(int categoryId)
        {
            try
            {
                _logger.Info("Retrieving room category by ID {CategoryId}", categoryId);
                var operationResult = await _roomCategoryRepository.GetByIdAsync(categoryId);

                if (!operationResult.IsSuccess)
                    _logger.ErrorNoEx($"An error has occurred on _roomCategoryRepository.GetByIdAsync({categoryId}) while retrieving a room category: {operationResult.Message}");

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving a room category by ID");
                return OperationResult<RoomCategoryDto>.Failure($"Error retrieving a room category: {ex.Message}");
            }
        }

        public async Task<OperationResult<ModifyRoomCategoryDto>> UpdateRoomCategory(ModifyRoomCategoryDto modifyRoomCategoryDto)
        {
            try
            {
                _logger.Info("Updating room category ID {CategoryId}", modifyRoomCategoryDto.CategoryId);

                if (modifyRoomCategoryDto is null)
                    return OperationResult<ModifyRoomCategoryDto>.Failure("ModifyRoomCategoryDto is required.");

                //Caso de Uso: RoomCategoryNameMustBeUnique (para modificación)
                var uniqueNameValidation = await _roomCategoryNameMustBeUnique.ValidateModify(modifyRoomCategoryDto.CategoryId, modifyRoomCategoryDto.Name);
                if (!uniqueNameValidation.IsSuccess)
                {
                    _logger.ErrorNoEx($"Validation failed for UpdateRoomCategory: {uniqueNameValidation.Message}");
                    return OperationResult<ModifyRoomCategoryDto>.Failure(uniqueNameValidation.Message);
                }
                //Fin Caso de Uso

                var operationResult = await _roomCategoryRepository.UpdateAsync(modifyRoomCategoryDto);

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
                return OperationResult<ModifyRoomCategoryDto>.Failure($"Error trying to update a room category: {ex.Message}");
            }
        }
    }
}