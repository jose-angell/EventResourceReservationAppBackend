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
    public class CreateReservationDetailUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReservationDetailUseCase> _logger;
        public CreateReservationDetailUseCase(IUnitOfWork unitOfWork, ILogger<CreateReservationDetailUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ReservationDetailsResponse>> ExecuteAsync(CreateReservationDetailRequest request)
        {
            try
            {
                var reservaDetail = await _unitOfWork.ReservationDetails.GetFirstOrDefaultAsync(r => r.ReservationId == request.ReservationId && r.ResourceId == request.ResourceId);
                if(reservaDetail != null)
                {
                    _logger.LogWarning("Fallo al crear el detalle de la reservacion: El recurso ya está asociado a la reservación.");
                    return OperationResult<ReservationDetailsResponse>.Failure(
                        "El recurso ya está asociado a la reservación.",
                        "Conflict",
                        "La operación de creación falló debido a que el recurso ya está asociado a la reservación."
                    );
                }
                var reservationDetail = new ReservationDetail(request.ReservationId, request.ResourceId, request.Quantity, request.UnitPrice);
                await _unitOfWork.ReservationDetails.AddAsync(reservationDetail);
                await _unitOfWork.SaveAsync();
                var response = new ReservationDetailsResponse
                {
                    Id = reservationDetail.Id,
                    ReservationId = reservationDetail.ReservationId,
                    ResourceId = reservationDetail.ResourceId,
                    Quantity = reservationDetail.Quantity,
                    UnitPrice = reservationDetail.UnitPrice
                };
                return OperationResult<ReservationDetailsResponse>.Success(response, "Detalle de la reservación creado exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al crear el detalle de la reservacion debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult<ReservationDetailsResponse>.Failure(
                    "Argumentos inválidos proporcionados para crear el detalle de la reservación.",
                    "BadRequest",
                    argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al crear el detalle de la reserva debido a un error de persistencia.");
                return OperationResult<ReservationDetailsResponse>.Failure(
                    "No se pudo guardar el detalle de la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de creación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al crear el detalle de la reserfva.");
                return OperationResult<ReservationDetailsResponse>.Failure(
                   "Ocurrió un error inesperado durante la creación de el detalle de la reserva.",
                    "UnexpectedError",
                    "La operación de creación falló debido a un error inesperado."
                );
            }
        }
    }
}
