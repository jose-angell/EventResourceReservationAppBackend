using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.ReservationDetails;
using EventResourceReservationApp.Application.DTOs.Reservations;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.ReservationDetails
{
    public class ReadReservationDetailUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadReservationDetailUseCase> _logger;
        public ReadReservationDetailUseCase(IUnitOfWork unitOfWork, ILogger<ReadReservationDetailUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<ReservationDetailsResponse>>> ExecuteAsync(Guid ReservationId)
        {
            try
            {
                var reservationDetails = await _unitOfWork.ReservationDetails.GetAllAsync(
                    filter: rd => rd.ReservationId == ReservationId,
                    includeProperties: "Resource,Reservation"
                    );
                var response = reservationDetails.Select(r => new ReservationDetailsResponse
                {
                    Id = r.Id,
                    ReservationId = r.ReservationId,
                    ResourceId = r.ResourceId,
                    ResourceName = r.Resource?.Name,
                    Quantity = r.Quantity,
                    UnitPrice = r.UnitPrice,
                    TotalPrice = r.Quantity * r.UnitPrice,
                    Created = r.Created
                });

                return OperationResult<IEnumerable<ReservationDetailsResponse>>.Success(response, "Detalle de reserva consultado exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al consultar el detalle de la reserva debido a un error de persistencia.");
                return OperationResult<IEnumerable<ReservationDetailsResponse>>.Failure(
                    "No se pudo consultar el detalle de la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de consulta falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al consultar el detalle de la reserfva.");
                return OperationResult<IEnumerable<ReservationDetailsResponse>>.Failure(
                   "Ocurrió un error inesperado durante la consulta del detalle dela reserva.",
                    "UnexpectedError",
                    "La operación de consulta falló debido a un error inesperado."
                );
            }
        }
    }
}
