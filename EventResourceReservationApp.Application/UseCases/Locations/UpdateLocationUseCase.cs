using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Loctions;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Locations
{
    public class UpdateLocationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateLocationUseCase> _logger;

        public UpdateLocationUseCase(IUnitOfWork unitOfWork, ILogger<UpdateLocationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(UpdateLocationRequest request)
        {
            Location updateLocation = await _unitOfWork.Locations.GetByIdAsync(request.Id);
            if (updateLocation == null)
            {
                _logger.LogWarning($"Fallo al actualizar: No se encontró una ubicación con el ID '{request.Id}'.");
                return OperationResult.Failure($"No se encontró una ubicación con el ID '{request.Id}'.",
                        "NotFound",
                        "La operación de actualización falló porque la ubicación no existe."
                );
            }
            try
            {
                updateLocation.Update(request.Country, request.City, request.ZipCode, request.Street, request.Neighborhood,
                    request.ExteriorNumber, request.InteriorNumber);
                await _unitOfWork.Locations.UpdateAsync(updateLocation);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Ubicación actualizada exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al actualizar ubicación debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult.Failure(
                    "La operación de actualización falló debido a una entrada inválida.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al actualizar ubicación debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo guardar la ubicación en la base de datos.",
                    "PersistenceError",
                    "La operación de actualización falló debido a un error de persistencia."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al actualizar ubicación debido a un error inesperado.");
                return OperationResult.Failure("Ocurrió un error inesperado al actualizar la ubicación.",
                    "UnexpectedError",
                    "La operación de actualización falló debido a un error inesperado."
                );
            }
        }
    }
}
