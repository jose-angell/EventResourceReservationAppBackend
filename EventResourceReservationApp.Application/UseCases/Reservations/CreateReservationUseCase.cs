using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reservation;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reservations
{
    public class CreateReservationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReservationUseCase> _logger;
        public CreateReservationUseCase(IUnitOfWork unitOfWork, ILogger<CreateReservationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ReservationResponse>> ExecuteAsync(CreateReservationRequest request)
        {
            try
            {
                var reservation = new Reservation(request.StartTime, request.EndTime, request.TotalAmount, request.ClientComment,
                    request.ClientPhoneNumber, request.LocationId, request.ClientId);
                await _unitOfWork.Reservations.AddAsync(reservation);
                await _unitOfWork.SaveAsync();
                var response = new ReservationResponse
                {
                    Id = reservation.Id,
                    ClientId = reservation.ClientId,
                    ClientName = "",
                    ClientPhoneNumber = reservation.ClientPhoneNumber,
                    StartTime = reservation.StartTime,
                    EndTime = reservation.EndTime,
                    Quantity = 0,
                    TotalAmount = reservation.TotalAmount,
                    StatusId = reservation.StatusId,
                    Status = ""
                };
                return OperationResult<ReservationResponse>.Success(response);
            }
            catch(ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al crear la reservacion debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult<ReservationResponse>.Failure(
                    "Argumentos inválidos proporcionados para crear la reservación.",
                    "BadRequest",
                    argEx.Message
                );
            }
            catch(PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al crear la reserva debido a un error de persistencia.");
                return OperationResult<ReservationResponse>.Failure(
                    "No se pudo guardar la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de creación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch(Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al crear la reserfva.");
                return OperationResult<ReservationResponse>.Failure(
                   "Ocurrió un error inesperado durante la creación de a reserva.",
                    "UnexpectedError",
                    "La operación de creación falló debido a un error inesperado."
                );
            }
        } 
    }
}
