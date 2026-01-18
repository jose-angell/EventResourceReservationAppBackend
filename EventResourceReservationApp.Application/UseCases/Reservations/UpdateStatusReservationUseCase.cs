using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reservations;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reservations
{
    public class UpdateStatusReservationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateStatusReservationUseCase> _logger;   
        public UpdateStatusReservationUseCase(IUnitOfWork unitOfWork, ILogger<UpdateStatusReservationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(UpdateStatusReservationRequest request)
        {
            try
            {
                var updateReservation = await _unitOfWork.Reservations.GetByIdAsync(request.Id);
                if (updateReservation == null)
                {
                    _logger.LogWarning($"Fallo al editar reserva: No se encontro la reserva con Id '{request.Id}'");
                    return OperationResult.Failure(
                        $"No se encontro la reserva con Id '{request.Id}'",
                    "NotFound",
                        $"La operación de edición falló debido a que no se encontró la reserva con Id '{request.Id}'."
                    );
                }
                updateReservation.UpdateStatus(request.StatusId, request.AdminId, request.AdminComment);
                await _unitOfWork.Reservations.UpdateAsync(updateReservation);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Reserva editada exitosamente.");    
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al editar la reservacion debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult.Failure(
                    "Argumentos inválidos proporcionados para editar la reservación.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al editar la reserva debido a un error de persistencia.");
                return OperationResult.Failure(
                    "No se pudo editar la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de edición falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al editar la reserfva.");
                return OperationResult.Failure(
                   "Ocurrió un error inesperado durante la edición de la reserva.",
                    "UnexpectedError",
                    "La operación de edición falló debido a un error inesperado."
                );
            }
        }
    }
}
