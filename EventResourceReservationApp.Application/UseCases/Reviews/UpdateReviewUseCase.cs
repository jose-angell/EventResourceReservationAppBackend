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
    public class UpdateReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateReviewUseCase> _logger;
        public UpdateReviewUseCase(IUnitOfWork unitOfWork, ILogger<UpdateReviewUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(UpdateReviewRequest request)
        {
            var reviewExisting = await _unitOfWork.Reviews.GetById(request.Id);
            if (reviewExisting == null)
            {
                _logger.LogWarning("Reseña con Id {ReviewId} no encontrada para actualización.", request.Id);
                return OperationResult.Failure("Reseña no encontrada.", "NotFound", $"No se encontró una reseña con Id {request.Id}.");
            }
            try
            {
                reviewExisting.Update(request.Rating, request.Comment);
                _unitOfWork.Reviews.UpdateAsync(reviewExisting);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Reseña actualizada exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al actualizar reseña debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult.Failure(
                    "La operación de actualización falló debido a una entrada inválida.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch(PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al actualizar reseña debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo actualizar la reseña en la base de datos.",
                    "PersistenceError",
                    "La operación de actualización falló debido a un problema de almacenamiento de datos.");
            }
            catch(Exception ex)
            {
                _logger.LogError(500, ex, "Fallo al actualizar reseña debido a un error inesperado.");
                return OperationResult.Failure("Ocurrió un error inesperado al actualizar la reseña.",
                    "UnexpectedError",
                    "La operación de actualización falló debido a un error inesperado.");
            }
        }
    }
}
