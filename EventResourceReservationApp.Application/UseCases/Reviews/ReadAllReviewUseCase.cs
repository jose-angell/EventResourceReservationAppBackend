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
    public class ReadAllReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadAllReviewUseCase> _logger;
        public ReadAllReviewUseCase(IUnitOfWork unitOfWork, ILogger<ReadAllReviewUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<ReviewResponse>>> ExecuteAsync(ReadReviewRequest request)
        {
            try
            {
                var reviews = await _unitOfWork.Reviews.GetAllAsync(
                    filter: r =>
                        (request.ResourceId == Guid.Empty || r.ResourceId == request.ResourceId) &&
                        (request.UserId == Guid.Empty || r.UserId == request.UserId) &&
                        (request.Rating == 0 || r.Rating == request.Rating) &&
                        (request.CreatedAt == default(DateTime) || r.CreatedAt.Date == request.CreatedAt.Date),
                    orderBy: q => q.OrderBy(r => r.CreatedAt)
                );
                var response = reviews.Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    ResourceId = r.ResourceId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                });
                return OperationResult<IEnumerable<ReviewResponse>>.Success(response, "Reseñas obtenidas exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al obtener las reseñas debido a un error de persistencia.");
                return OperationResult<IEnumerable<ReviewResponse>>.Failure(
                    "No se pudieron obtener las reseñas de la base de datos.",
                    "PersistenceError",
                    "La operación de lectura falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al obtener las reseñas debido a un error inesperado.");
                return OperationResult<IEnumerable<ReviewResponse>>.Failure(
                    "Ocurrió un error inesperado al obtener las reseñas.",
                    "UnexpectedError",
                    "La operación de lectura falló debido a un error inesperado."
                );
            }
        }
    }
}
