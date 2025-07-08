using Microsoft.Extensions.Configuration;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Domain.Base;
using SGRH.Application.UseCases.Hotel.Floor;
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
        private readonly FloorNumberMustBeUnique _floorNumberMustBeUnique; 
        private readonly FloorMustNotHaveActiveReservations _floorMustNotHaveActiveReservations; 

        public FloorService(
            IFloorRepository floorRepository,
            IAppLogger<FloorService> logger,
            IConfiguration configuration,
            FloorNumberMustBeUnique floorNumberMustBeUnique, 
            FloorMustNotHaveActiveReservations floorMustNotHaveActiveReservations 
        )
        {
            _floorRepository = floorRepository;
            _logger = logger;
            _configuration = configuration;
            _floorNumberMustBeUnique = floorNumberMustBeUnique;
            _floorMustNotHaveActiveReservations = floorMustNotHaveActiveReservations;
        }

        public async Task<OperationResult<CreateFloorDto>> CreateFloor(CreateFloorDto createFloorDto)
        {
            try
            {
                _logger.Info("Creating floor", createFloorDto);

                if (createFloorDto is null)
                    return OperationResult<CreateFloorDto>.Failure("CreateFloorDto is required.");

                //Caso de Uso: FloorNumberMustBeUnique (para creación)
                var uniqueNumberValidation = await _floorNumberMustBeUnique.ValidateCreate(createFloorDto.FloorNumber);
                if (!uniqueNumberValidation.IsSuccess)
                {
                    _logger.ErrorNoEx($"Validation failed for CreateFloor: {uniqueNumberValidation.Message}");
                    return OperationResult<CreateFloorDto>.Failure(uniqueNumberValidation.Message);
                }
                //Fin Caso de Uso

                var operationResult = await _floorRepository.AddAsync(createFloorDto);

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
                return OperationResult<CreateFloorDto>.Failure($"Error creating a floor: {ex.Message}");
            }
        }

        public async Task<OperationResult<DisableFloorDto>> DeleteFloor(DisableFloorDto disableFloorDto)
        {
            try
            {
                _logger.Info("Disabling floor ID {FloorId}", disableFloorDto.FloorId);

                if (disableFloorDto is null)
                    return OperationResult<DisableFloorDto>.Failure("DisableFloorDto is required.");

                //Caso de Uso: FloorMustNotHaveActiveReservations
                var activeReservationsValidation = await _floorMustNotHaveActiveReservations.Validate(disableFloorDto.FloorId);
                if (!activeReservationsValidation.IsSuccess)
                {
                    _logger.ErrorNoEx($"Validation failed for DeleteFloor: {activeReservationsValidation.Message}");
                    return OperationResult<DisableFloorDto>.Failure(activeReservationsValidation.Message);
                }
                //Fin Caso de Uso

                var operationResult = await _floorRepository.DeleteAsync(disableFloorDto);

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
                return OperationResult<DisableFloorDto>.Failure($"Error trying to disable a floor: {ex.Message}");
            }
        }

        //(GetFloors y GetFloorsById solo lectura)
        public async Task<OperationResult<IEnumerable<FloorDto>>> GetFloors()
        {
            try
            {
                _logger.Info("Retrieving all floors");
                var operationResult = await _floorRepository.GetAllAsync();

                if (!operationResult.IsSuccess)
                    _logger.ErrorNoEx($"An error has occurred on _floorRepository.GetAllAsync() while retrieving floors: {operationResult.Message}");

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving floors");
                return OperationResult<IEnumerable<FloorDto>>.Failure($"Error retrieving floors: {ex.Message}");
            }
        }

        public async Task<OperationResult<FloorDto>> GetFloorsById(int floorId)
        {
            try
            {
                _logger.Info("Retrieving floor by ID {FloorId}", floorId);
                var operationResult = await _floorRepository.GetByIdAsync(floorId);

                if (!operationResult.IsSuccess)
                    _logger.ErrorNoEx($"An error has occurred on _floorRepository.GetByIdAsync({floorId}) while retrieving a floor: {operationResult.Message}");

                return operationResult;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex, "Error retrieving a floor by ID");
                return OperationResult<FloorDto>.Failure($"Error retrieving a floor: {ex.Message}");
            }
        }

        public async Task<OperationResult<ModifyFloorDto>> UpdateFloor(ModifyFloorDto modifyFloorDto)
        {
            try
            {
                _logger.Info("Updating floor ID {FloorId}", modifyFloorDto.FloorId);

                if (modifyFloorDto is null)
                    return OperationResult<ModifyFloorDto>.Failure("ModifyFloorDto is required.");

                //Caso de Uso: FloorNumberMustBeUnique (para modificación)
                var uniqueNumberValidation = await _floorNumberMustBeUnique.ValidateModify(modifyFloorDto.FloorId, modifyFloorDto.FloorNumber);
                if (!uniqueNumberValidation.IsSuccess)
                {
                    _logger.ErrorNoEx($"Validation failed for UpdateFloor: {uniqueNumberValidation.Message}");
                    return OperationResult<ModifyFloorDto>.Failure(uniqueNumberValidation.Message);
                }
                //Fin Caso de Uso

                var operationResult = await _floorRepository.UpdateAsync(modifyFloorDto);

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
                return OperationResult<ModifyFloorDto>.Failure($"Error trying to update a floor: {ex.Message}");
            }
        }
    }
}