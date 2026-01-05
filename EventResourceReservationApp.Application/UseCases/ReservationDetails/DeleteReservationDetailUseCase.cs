using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.ReservationDetails
{
    public class DeleteReservationDetailUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteReservationDetailUseCase> _logger;
        public DeleteReservationDetailUseCase(IUnitOfWork unitOfWork, ILogger<DeleteReservationDetailUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(Guid reservationDetailId)
        {
            try
            {
                var deleteReservationDetail = await _unitOfWork.ReservationDetails.GetByIdAsync(reservationDetailId);
                if (deleteReservationDetail == null)
                {
                    _logger.LogWarning($"Fallo al eliminar el detalle de la reserva: No se encontro la reserva con Id '{reservationDetailId}'");
                    return OperationResult.Failure(
                        $"No se encontro el detalle de la reserva con Id '{reservationDetailId}'",
                    "NotFound",
                        $"La operación de eliminación falló debido a que no se encontró el detalle de la reserva con Id '{reservationDetailId}'."
                    );
                }
                // TODO: eliminar todos los detalles de la reserva asociados antes de eliminar la reserva.
                await _unitOfWork.ReservationDetails.RemoveAsync(deleteReservationDetail);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Detalle de la reserva eliminada exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al eliminar el detalle de la reserva debido a un error de persistencia.");
                return OperationResult.Failure(
                    "Error al eliminar el detalle de la reservación debido a un problema de persistencia.",
                    "PersistenceError",
                    "La operación de eliminación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al eliminar el detalle de la reserva.");
                return OperationResult.Failure(
                   "Ocurrió un error inesperado durante la eliminación del detalle de la reserva.",
                    "UnexpectedError",
                    "La operación de eliminación falló debido a un error inesperado."
                );
            }
        }
    }
}
