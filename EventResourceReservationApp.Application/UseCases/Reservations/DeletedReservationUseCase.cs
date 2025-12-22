using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reservation;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reservations
{
    public class DeletedReservationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletedReservationUseCase> _logger;
        public DeletedReservationUseCase(IUnitOfWork unitOfWork, ILogger<DeletedReservationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(Guid reservationId)
        {
            try
            {
                var deleteReservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
                if (deleteReservation == null)
                {
                    _logger.LogWarning($"Fallo al eliminar reserva: No se encontro la reserva con Id '{reservationId}'");
                    return OperationResult.Failure(
                        $"No se encontro la reserva con Id '{reservationId}'",
                        "NotFound",
                        $"La operación de eliminación falló debido a que no se encontró la reserva con Id '{reservationId}'."
                    );
                }
                // TODO: eliminar todos los detalles de la reserva asociados antes de eliminar la reserva.
                await _unitOfWork.Reservations.RemoveAsync(deleteReservation);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Reserva eliminada exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al eliminar la reserva debido a un error de persistencia.");
                return OperationResult.Failure(
                    "Error al eliminar la reservación debido a un problema de persistencia.",
                    "PersistenceError",
                    "La operación de eliminación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al eliminar la reserfva.");
                return OperationResult.Failure(
                   "Ocurrió un error inesperado durante la eliminación de la reserva.",
                    "UnexpectedError",
                    "La operación de eliminación falló debido a un error inesperado."
                );
            }
        }
    }
}
