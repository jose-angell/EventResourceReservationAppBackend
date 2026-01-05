using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reservation;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Reservations
{
    public class ReadAllReservationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadAllReservationUseCase> _logger;
        public ReadAllReservationUseCase(IUnitOfWork unitOfWork, ILogger<ReadAllReservationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<ReservationResponse>>> ExecuteAsync(ReadAllReservationRequest request)
        {
            try
            {
                var reservations = await _unitOfWork.Reservations.GetAllAsync(
                    filter: r => (request.StatusId == null || r.StatusId == request.StatusId) &&
                        (request.CreatedByUserIdFilter == null || r.ClientId == request.CreatedByUserIdFilter),
                    orderBy: q =>
                    {
                        return request.OrderBy?.ToLower() switch
                        {
                            "createdAt_asc" => q.OrderBy(r => r.CreatedAt),
                            "createdAt_desc" => q.OrderByDescending(r => r.CreatedAt),
                            _ => q.OrderByDescending(r => r.CreatedAt),
                        };
                    },
                    includeProperties: "Resource,Client,Admin,ReservationDetail"
                );
                var response = reservations.Select(r => new ReservationResponse
                {
                    Id = r.Id,
                    ClientId = r.ClientId,
                    ClientName = r.Client.FirsName,
                    ClientPhoneNumber = r.Client.PhoneNumber,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    Quantity = 0, //TODO es la cantidad de recursos reservados contar solo dos tipos
                    TotalAmount = r.TotalAmount,
                    StatusId = r.StatusId,
                    Status = ""
                });
                return OperationResult<IEnumerable<ReservationResponse>>.Success(response, "Reservas consultadas exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al consultar la reserva debido a un error de persistencia.");
                return OperationResult<IEnumerable<ReservationResponse>>.Failure(
                    "No se pudo consultar la reservacion en la base de datos.",
                    "PersistenceError",
                    "La operación de consulta falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al consultar la reserfva.");
                return OperationResult<IEnumerable<ReservationResponse>>.Failure(
                   "Ocurrió un error inesperado durante la consulta de la reserva.",
                    "UnexpectedError",
                    "La operación de consulta falló debido a un error inesperado."
                );
            }
        }
    }
}
