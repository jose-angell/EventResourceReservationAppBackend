using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.DTOs.ReservationCarItems;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.ReservationCarItems
{
    public class CreateReservationCarItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReservationCarItemUseCase> _logger;
        public CreateReservationCarItemUseCase(IUnitOfWork unitOfWork, ILogger<CreateReservationCarItemUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ReservationCarItemResponse>> ExecuteAsync(CreateReservationCarItemRequest request)
        {
            
            var existingItem = await _unitOfWork.ReservationCarItems.GetByClientIdAndResourceIdAsync(request.ClientId,request.ResourceId);
            if (existingItem != null)
            {
                _logger.LogWarning("Fallo al crear nuevo elemento en Carrito de reservas: Ya existe para el mismo carrito un recurso con el id '{ResourceId}'.", request.ResourceId);
                return OperationResult<ReservationCarItemResponse>.Failure(
                    $"Ya existe una el Recurso con el Id '{request.ResourceId}'.",
                    "Conflict",
                    $"La operación de Creacion falló debido a una duplicación de recurso para un carrito de reservas '{request.ResourceId}'."
                );
            }
            try
            {
                var newItem = new ReservationCarItem(
                    clientId: request.ClientId,
                    resourceId: request.ResourceId,
                    quantity: request.Quantity
                );
                await _unitOfWork.ReservationCarItems.AddAsync(newItem);
                await _unitOfWork.SaveAsync();
                var response = new ReservationCarItemResponse
                {
                    Id = newItem.Id,
                    ClientId = newItem.ClientId,
                    ResourceId = newItem.ResourceId,
                    Quantity = newItem.Quantity,
                    AddedAt = newItem.AddedAt
                };
                return OperationResult<ReservationCarItemResponse>.Success(response, "Elemento agregado exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al crear el elemento de Carrito de reservas debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult<ReservationCarItemResponse>.Failure(
                    "La operación de creación falló debido a una entrada inválida.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch(PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al crear el elemento de Carrito de reservas debido a un error de persistencia.");
                return OperationResult<ReservationCarItemResponse>.Failure("No se pudo guardar el elemento en la base de datos.",
                    "PersistenceError",
                    "Ocurrió un error al intentar guardar el elemento. Por favor, inténtelo de nuevo más tarde.");
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al crear el elemento de Carrito de reservas.");
                return OperationResult<ReservationCarItemResponse>.Failure("Ocurrió un error inesperado.",
                    "UnexpectedError",
                    "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.");
            }
        }
    }
}
