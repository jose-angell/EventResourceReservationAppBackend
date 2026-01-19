using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.ReservationDetails;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.ReservationDetails
{
    public class UpdateReservationDetailUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateReservationDetailUseCase> _logger;
        public UpdateReservationDetailUseCase(IUnitOfWork unitOfWork, ILogger<UpdateReservationDetailUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(UpdateReservationDetailRequest request)
        {
            try
            {

                var reservationDetail = await _unitOfWork.ReservationDetails.GetByIdAsync(request.Id);
                if (reservationDetail == null)
                {
                    _logger.LogWarning($"Fallo al editar el detalle de la reservacion: No se encontro el detalle con Id '{request.Id}'");
                    return OperationResult.Failure(
                        $"No se encontro el detalle de la reservacion con Id '{request.Id}'",
                        "NotFound",
                        $"La operación de edición falló debido a que no se encontró el detalle de la reservacion con Id '{request.Id}'."
                    );
                }
                reservationDetail.UpdateQuantity(request.Quantity);
                await _unitOfWork.ReservationDetails.Update(reservationDetail);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Detalle de la reservacion editado exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al editar el detalle de la reservacion debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult.Failure(
                    "Argumentos inválidos proporcionados para editar el detalle de la reservación.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al editar el detalle de la reserva debido a un error de persistencia.");
                return OperationResult.Failure(
                    "No se pudo editar el detalle de la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de edición falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al editar el detalle de la reserfva.");
                return OperationResult.Failure(
                   "Ocurrió un error inesperado durante la edición del detalle de la reserva.",
                    "UnexpectedError",
                    "La operación de edición falló debido a un error inesperado."
                );
            }
        }
    }
}
