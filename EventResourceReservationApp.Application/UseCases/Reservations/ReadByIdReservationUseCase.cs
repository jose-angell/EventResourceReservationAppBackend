using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reservation;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reservations
{
    public class ReadByIdReservationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadByIdReservationUseCase> _logger;
        public ReadByIdReservationUseCase(IUnitOfWork unitOfWork, ILogger<ReadByIdReservationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ReservationResponse>> ExecuteAsync(Guid reservationId)
        {
            try
            {
                var reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
                if (reservation == null) 
                {
                    _logger.LogWarning($"Fallo al consutlar reserva: No se encontro la reserva con Id '{reservationId}'");
                    return OperationResult<ReservationResponse>.Failure(
                        $"No se encontro la reserva con Id '{reservationId}'",
                        "NotFound",
                        $"La operación de consulta falló debido a que no se encontró la reserva con Id '{reservationId}'."
                    );
                }
                var response = new ReservationResponse
                {
                    Id = reservation.Id,
                    ClientId = reservation.ClientId,
                    ClientName = reservation.Client.FirsName,
                    ClientPhoneNumber = reservation.Client.PhoneNumber,
                    StartTime = reservation.StartTime,
                    EndTime = reservation.EndTime,
                    Quantity = 0, //TODO es la cantidad de recursos reservados contar solo dos tipos
                    TotalAmount = reservation.TotalAmount,
                    StatusId = reservation.StatusId,
                    Status = ""
                };
                return OperationResult<ReservationResponse>.Success(response, "Reserva consultada exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al consultar la reserva debido a un error de persistencia.");
                return OperationResult<ReservationResponse>.Failure(
                    "No se pudo consultar la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de consulta falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al consultar la reserfva.");
                return OperationResult<ReservationResponse>.Failure(
                   "Ocurrió un error inesperado durante la consulta de la reserva.",
                    "UnexpectedError",
                    "La operación de consulta falló debido a un error inesperado."
                );
            }
        }
    }
}
