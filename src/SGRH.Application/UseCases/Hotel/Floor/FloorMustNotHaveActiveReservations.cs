using SGRH.Domain.Base;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using System.Threading.Tasks;

namespace SGRH.Application.UseCases.Hotel.Floor
{
    public class FloorMustNotHaveActiveReservations
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IReservationRepository _reservationRepository;

        public FloorMustNotHaveActiveReservations(
            IFloorRepository floorRepository,
            IReservationRepository reservationRepository
            )
        {
            _floorRepository = floorRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<OperationResult<string>> Validate(int floorId)
        {
            //Verificar si el piso existe
            var floorResult = await _floorRepository.GetByIdAsync(floorId);
            if (!floorResult.IsSuccess || floorResult.Data == null)
            {
                return OperationResult<string>.Failure($"FloorId {floorId} does not exist.");
            }

            // apatir de aqui hare una espiece de simulacion hasta tener la logica de reservas implementada en esta rama
            // por que se necesita verificar si hay reservas activas asociadas a este piso

            //var hasActiveReservations = await _reservationRepository.CheckActiveReservationsByFloorId(floorId); (comentado a modo de place holder)

            bool hasActiveReservations = false; // <--- CAMBIAR ESTO LUEGO CON LA LOGICA REAL DE RESERVAS

            // Si el FloorId es 99, simulamos que tiene reservas activas para la prueba
            if (floorId == 99) // Quitar esta línea de simulación en producción
            {
                hasActiveReservations = true; // Quitar esta línea de simulación en producción
            }


            if (hasActiveReservations)
            {
                return OperationResult<string>.Failure($"Floor {floorId} cannot be deleted because it has active reservations.");
            }

            return OperationResult<string>.Success("Floor has no active reservations.");
        }
    }
}