using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reviews
{
    public class DeleteReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteReviewUseCase> _logger;
        public DeleteReviewUseCase(IUnitOfWork unitOfWork, ILogger<DeleteReviewUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(Guid reviewId)
        {
            var reviewExisting = await _unitOfWork.Reviews.GetById(reviewId);
            if (reviewExisting == null)
            {
                _logger.LogWarning("Reseña con Id {ReviewId} no encontrada para actualización.", reviewId);
                return OperationResult.Failure("Reseña no encontrada.", "NotFound", $"No se encontró una reseña con Id {reviewId}.");
            }
            try
            {
                _unitOfWork.Reviews.RemoveASync(reviewExisting);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Reseña eliminada exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al eliminar reseña debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo eliminar la reseña en la base de datos.",
                    "PersistenceError",
                    "La operación de eliminación falló debido a un problema de almacenamiento de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo al eliminar reseña debido a un error inesperado.");
                return OperationResult.Failure("Ocurrió un error inesperado al eliminar la reseña.",
                    "UnexpectedError",
                    "La operación de eliminación falló debido a un error inesperado.");
            }
        }
    }
}
