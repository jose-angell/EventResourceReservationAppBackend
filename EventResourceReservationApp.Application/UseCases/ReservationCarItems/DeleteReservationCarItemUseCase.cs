using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.ReservationCarItems
{
    public class DeleteReservationCarItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteReservationCarItemUseCase> _logger;
        public DeleteReservationCarItemUseCase(IUnitOfWork unitOfWork, ILogger<DeleteReservationCarItemUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(Guid itemId)
        {
            var existingItem = await _unitOfWork.ReservationCarItems.GetByIdAsync(itemId);
            if (existingItem == null)
            {
                _logger.LogWarning("Fallo al eliminar el elemento de Carrito de reservas: No se encontró el elemento con Id '{Id}'.", itemId);
                return OperationResult.Failure(
                    $"No se encontró el elemento con Id '{itemId}'.",
                    "NotFound",
                    "La operación de eliminación falló porque el elemento no existe."
                );
            }
            try
            {
                await _unitOfWork.ReservationCarItems.RemoveASync(existingItem);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Elemento eliminado exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al eliminar el elemento de carrito de reserva debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo eliminar el elemento de carrito de reserva de la base de datos.",
                    "PersistenceError",
                    "La operación de eliminación falló debido a un problema de almacenamiento de datos.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la eliminación del elemento de carrito de reserva en el caso de uso.");
                return OperationResult.Failure("Ocurrio un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de eliminación falló debido a un problema inesperado.");
            }
        }
    }
}
