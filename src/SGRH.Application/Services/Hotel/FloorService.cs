using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Application.Services.Hotel
{
    public sealed class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IAppLogger<FloorService> _logger;
        private readonly IConfiguration _configuration;

        public FloorService(IFloorRepository floorRepository, IAppLogger<FloorService> logger, IConfiguration configuration)
        {
            _floorRepository = floorRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<CreateFloorDto>> CreateFloor(CreateFloorDto createFloorDto)
        {
            OperationResult<CreateFloorDto> operationResult = new OperationResult<CreateFloorDto>();
            try
            {
                _logger.Info("Creating floor", createFloorDto);

                if (createFloorDto is null)
                {
                    operationResult = OperationResult<CreateFloorDto>.Failure("Object CreateFloorDto is required.");
                    return operationResult;
                }

                operationResult = await _floorRepository.AddAsync(createFloorDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred while creating: {operationResult.Message} Persisting floor.");
                    return operationResult;
                }

                _logger.Info($"The floor number {createFloorDto.FloorNumber} was created successfully.", createFloorDto);
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error creating a floor");
                operationResult = OperationResult<CreateFloorDto>.Failure($"Error creating a floor: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<DisableFloorDto>> DeleteFloor(DisableFloorDto disableFloorDto)
        {
            OperationResult<DisableFloorDto> operationResult = new OperationResult<DisableFloorDto>();
            try
            {
                _logger.Info("Disabling floor ID {FloorId}", disableFloorDto.FloorId);

                if (disableFloorDto is null)
                {
                    operationResult = OperationResult<DisableFloorDto>.Failure("Object DisableFloorDto is required.");
                    return operationResult;
                }

                operationResult = await _floorRepository.DeleteAsync(disableFloorDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred while disabling floor: {operationResult.Message} Persisting floor.");
                    return operationResult;
                }

                _logger.Info($"The floor ID {disableFloorDto.FloorId} was disabled successfully.");
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error disabling a floor");
                operationResult = OperationResult<DisableFloorDto>.Failure($"Error trying to disable a floor: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<IEnumerable<FloorDto>>> GetFloors()
        {
            OperationResult<IEnumerable<FloorDto>> operationResult = new OperationResult<IEnumerable<FloorDto>>();
            try
            {
                _logger.Info("Retrieving all floors");
                operationResult = await _floorRepository.GetAllAsync();

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred on _floorRepository.GetAllAsync() while retrieving floors: {operationResult.Message}");
                }

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving floors");
                operationResult = OperationResult<IEnumerable<FloorDto>>.Failure($"Error retrieving floors: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<FloorDto>> GetFloorsById(int floorId)
        {
            OperationResult<FloorDto> operationResult = new OperationResult<FloorDto>();
            try
            {
                _logger.Info("Retrieving floor by ID {FloorId}", floorId);
                operationResult = await _floorRepository.GetByIdAsync(floorId);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred on _floorRepository.GetByIdAsync({floorId}) while retrieving a floor: {operationResult.Message}");
                }
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving a floor by ID");
                operationResult = OperationResult<FloorDto>.Failure($"Error retrieving a floor: {ex.Message}");
                return operationResult;
            }
        }

        public async Task<OperationResult<ModifyFloorDto>> UpdateFloor(ModifyFloorDto modifyFloorDto)
        {
            OperationResult<ModifyFloorDto> operationResult = new OperationResult<ModifyFloorDto>();
            try
            {
                _logger.Info("Updating floor ID {FloorId}", modifyFloorDto.FloorId);

                if (modifyFloorDto is null)
                {
                    operationResult = OperationResult<ModifyFloorDto>.Failure("Object ModifyFloorDto is required.");
                    return operationResult;
                }

                operationResult = await _floorRepository.UpdateAsync(modifyFloorDto);

                if (!operationResult.IsSuccess)
                {
                    _logger.ErrorNoEx($"An error has occurred while updating floor: {operationResult.Message} Persisting floor.");
                    return operationResult;
                }

                _logger.Info($"The floor {modifyFloorDto.FloorNumber} was updated successfully.");
                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error during UpdateFloor");
                operationResult = OperationResult<ModifyFloorDto>.Failure($"Error trying to update a floor: {ex.Message}");
                return operationResult;
            }
        }
    }
}