using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
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
    public class UpdateReservationCarItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateReservationCarItemUseCase> _logger;
        public UpdateReservationCarItemUseCase(IUnitOfWork unitOfWork, ILogger<UpdateReservationCarItemUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> ExecuteAsync(UpdateReservationCarItemRequest request)
        {
            var existingItem = await _unitOfWork.ReservationCarItems.GetByIdAsync(request.Id);
            if (existingItem == null)
            {
           
                _logger.LogWarning("Fallo al actualizar el elemento de Carrito de reservas: No se encontró el elemento con Id '{Id}'.", request.Id);
                return OperationResult.Failure(
                    $"No se encontró el elemento con Id '{request.Id}'.",
                    "NotFound",
                    "La operación de actualización falló porque el elemento no existe."
                );
            }
            try
            {
                existingItem.UpdateQuantity(request.Quantity);
                await _unitOfWork.ReservationCarItems.UpdateAsync(existingItem);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Elemento actualizado exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al actualizar el elemento de Carrito de reservas debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult.Failure(
                    "La operación de creación falló debido a una entrada inválida.",
                     "InvalidInput",
                     argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al actualizar el elemento de Carrito de reservas debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo guardar los cambios de el elemento de Carrito de reservas en la base de datos.",
                    "PersistenceError",
                    "La operación de actualización falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el elemento de Carrito de reservas: {ErrorMessage}", ex.Message);
                return OperationResult.Failure(
                    "Ocurrió un error inesperado al procesar la solicitud.",
                    "UnexpectedError",
                    "La operación de actualización falló debido a un error del servidor."
                );
            }
        }
    }
}
