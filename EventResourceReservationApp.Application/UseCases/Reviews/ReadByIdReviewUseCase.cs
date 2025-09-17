using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reviews;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reviews
{
    public class ReadByIdReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadByIdReviewUseCase> _logger;    
        public ReadByIdReviewUseCase(IUnitOfWork unitOfWork, ILogger<ReadByIdReviewUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ReviewResponse>> ExecuteAsync(Guid reviewId)
        {
            try
            {
                var reviewExisting = await _unitOfWork.Reviews.GetById(reviewId);
                if (reviewExisting == null)
                {
                    _logger.LogWarning("Reseña con Id {ReviewId} no encontrada para actualización.", reviewId);
                    return OperationResult<ReviewResponse>.Failure("Reseña no encontrada.", "NotFound", $"No se encontró una reseña con Id {reviewId}.");
                }
                var response = new ReviewResponse
                {
                    Id = reviewExisting.Id,
                    ResourceId = reviewExisting.ResourceId,
                    UserId = reviewExisting.UserId,
                    Rating = reviewExisting.Rating,
                    Comment = reviewExisting.Comment,
                    CreatedAt = reviewExisting.CreatedAt
                };
                return OperationResult<ReviewResponse>.Success(response, "Reseña obtenida exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al obtener la reseña debido a un error de persistencia.");
                return OperationResult<ReviewResponse>.Failure(
                    "No se pudo obtener la reseña de la base de datos.",
                    "PersistenceError",
                    "La operación de lectura falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al obtener la reseña debido a un error inesperado.");
                return OperationResult<ReviewResponse>.Failure(
                    "Ocurrió un error inesperado al obtener la reseña.",
                    "UnexpectedError",
                    "La operación de lectura falló debido a un error inesperado."
                );
            }
        }
    }
}
