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
                        (request.ResourceId == null || r.ResourceId == request.ResourceId) &&
                        (request.UserId == null || r.UserId == request.UserId) &&
                        (request.Rating == null || r.Rating == request.Rating) &&
                        (request.CreatedAt == null ||  r.CreatedAt.Date == request.CreatedAt.Value.Date),
                    orderBy: q =>
                    {
                        return request.OrderBy?.ToLower() switch
                        {
                            "rating_asc" => q.OrderBy(r => r.Rating),
                            "rating_desc" => q.OrderByDescending(r => r.Rating),
                            "date_asc" => q.OrderBy(r => r.CreatedAt),
                            "date_desc" => q.OrderByDescending(r => r.CreatedAt),
                            _ => q.OrderByDescending(r => r.CreatedAt),
                        };
                    }
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
