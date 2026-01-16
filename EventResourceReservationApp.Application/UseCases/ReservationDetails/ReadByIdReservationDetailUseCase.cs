using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.ReservationDetails;
using EventResourceReservationApp.Application.DTOs.Reservations;
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
    public class ReadByIdReservationDetailUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadByIdReservationDetailUseCase> _logger;
        public ReadByIdReservationDetailUseCase(IUnitOfWork unitOfWork, ILogger<ReadByIdReservationDetailUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ReservationDetailsResponse>> ExecuteAsync(Guid ReservationDetailId)
        {
            try
            {
                var reservationDetail = await _unitOfWork.ReservationDetails.GetByIdAsync(ReservationDetailId);
                if (reservationDetail == null)
                {
                    _logger.LogWarning($"Fallo al consutlar el detalle de la reserva: No se encontro el detalle de la reserva con Id '{ReservationDetailId}'");
                    return OperationResult<ReservationDetailsResponse>.Failure(
                    $"No se encontro el detalle de la reserva con Id '{ReservationDetailId}'",
                    "NotFound",
                        $"La operación de consulta falló debido a que no se encontró el detalle de la reserva con Id '{ReservationDetailId}'."
                    );
                }
                var response = new ReservationDetailsResponse
                {
                    Id = reservationDetail.Id,
                    ReservationId = reservationDetail.ReservationId,
                    ResourceId = reservationDetail.ResourceId,
                    ResourceName = reservationDetail.Resource?.Name,
                    Quantity = reservationDetail.Quantity,
                    UnitPrice = reservationDetail.UnitPrice,
                    TotalPrice = reservationDetail.Quantity * reservationDetail.UnitPrice,
                    Created = reservationDetail.Created
                };
                return OperationResult<ReservationDetailsResponse>.Success(response, "Detalle de reserva consultada exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al consultar el detalle de la reserva debido a un error de persistencia.");
                return OperationResult<ReservationDetailsResponse>.Failure(
                    "No se pudo consultar el detall de la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de consulta falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al consultar el detalle de la reserva.");
                return OperationResult<ReservationDetailsResponse>.Failure(
                   "Ocurrió un error inesperado durante la consulta del detalle de la reserva.",
                    "UnexpectedError",
                    "La operación de consulta falló debido a un error inesperado."
                );
            }
        }
    }
}
