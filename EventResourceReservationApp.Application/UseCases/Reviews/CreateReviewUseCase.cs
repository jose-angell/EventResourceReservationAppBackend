using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reviews;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reviews
{
    public class CreateReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReviewUseCase> _logger;

        public CreateReviewUseCase(IUnitOfWork unitOrworkl, ILogger<CreateReviewUseCase> logger)
        {
            _unitOfWork = unitOrworkl;
            _logger = logger;
        }
        public async Task<OperationResult<ReviewResponse>> ExecuteAsync(CreateReviewRequest request)
        {
            try
            {
                Review newReview = new Review(request.ResourceId, request.UserId, request.Rating, request.Comment);
                await _unitOfWork.Reviews.AddAsync(newReview);
                await _unitOfWork.SaveAsync();
                ReviewResponse response = new ReviewResponse
                {
                    Id = newReview.Id,
                    ResourceId = newReview.ResourceId,
                    UserId = newReview.UserId,
                    Rating = newReview.Rating,
                    Comment = newReview.Comment,
                    CreatedAt = newReview.CreatedAt
                };
                return OperationResult<ReviewResponse>.Success(response, "Reseña creada exitosamente");
            }
            catch( ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al crear reseña debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult<ReviewResponse>.Failure(
                    "La operación de creación falló debido a una entrada inválida.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al crear reseña debido a un error de persistencia.");
                return OperationResult<ReviewResponse>.Failure("No se pudo guardar la reseña en la base de datos.",
                    "PersistenceError",
                    "La operación de creación falló debido a un problema de almacenamiento de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo al crear reseña debido a un error inesperado.");
                return OperationResult<ReviewResponse>.Failure("Ocurrió un error inesperado al crear la reseña.",
                    "UnexpectedError",
                    "La operación de creación falló debido a un error inesperado.");
            }
        }
    }
}
