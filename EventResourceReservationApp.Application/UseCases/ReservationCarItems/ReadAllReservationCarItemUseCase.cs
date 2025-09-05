using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.ReservationCarItems;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.ReservationCarItems
{
    public class ReadAllReservationCarItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadAllReservationCarItemUseCase> _logger;
        public ReadAllReservationCarItemUseCase(IUnitOfWork unitOfWork, ILogger<ReadAllReservationCarItemUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<ReservationCarItemResponse>>> ExecuteAsync()
        {
            try
            {
                var items = await _unitOfWork.ReservationCarItems.GetAllAsync();
                var response = items.Select(item => new ReservationCarItemResponse
                {
                    Id = item.Id,
                    ClientId = item.ClientId,
                    ResourceId = item.ResourceId,
                    Quantity = item.Quantity,
                    AddedAt = item.AddedAt
                });
                return OperationResult<IEnumerable<ReservationCarItemResponse>>.Success(response, "Elementos leídos exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al leer los elementos de Carrito de reservas debido a un error de persistencia.");
                return OperationResult<IEnumerable<ReservationCarItemResponse>>.Failure("No se pudo leer los elementos de Carrito de reservas de la base de datos.",
                    "PersistenceError",
                    "La operación de lectura falló debido a un problema de almacenamiento de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la lectura de los elementos de Carrito de reservas en el caso de uso.");
                return OperationResult<IEnumerable<ReservationCarItemResponse>>.Failure("Ocurrio un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de lectura falló debido a un problema inesperado.");
            }
        }
    }
}
