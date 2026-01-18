using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Resources
{
    public class UpdateResourceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateResourceUseCase> _logger;
        public UpdateResourceUseCase(IUnitOfWork unitOfWork, ILogger<UpdateResourceUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(UpdateResourceRequest request)
        {
            Resource updateResource = await _unitOfWork.Resources.GetByIdAsync(request.Id);
            if (updateResource == null)
            {
                _logger.LogWarning($"Fallo al actualizar recurso: No se enconto el recurso con Id '{request.Id}'");
                return OperationResult.Failure(
                    $"No se enconto el recurso con Id '{request.Id}'",
                    "NotFound",
                    $"La operación de actualización falló debido a que no se encontró el recurso con Id '{request.Id}'."
                );
            }
            try
            {
                if (await _unitOfWork.Resources.GetFirstOrDefaultAsync(r => r.Name.ToLower() == request.Name.ToLower() && r.LocationId == request.LocationId && r.Id != request.Id) != null) {
                    _logger.LogWarning($"Fallo al actulizar recurso: Ya existe un recurso con el nombre '{request.Name}' en la ubicación '{request.LocationId}'.");
                    return OperationResult<ResourceResponse>.Failure(
                        $"Ya existe un recurso con el nombre '{request.Name}' en la ubicación especificada.",
                        "Conflict",
                        $"La operación de actualización falló debido a una duplicación de nombre '{request.Name}' en la ubicación '{request.LocationId}'."
                    );
                }
                updateResource.Update(request.CategoryId, request.StatusId, request.Name, request.Description, request.Quantity,
                    request.Price, request.AuthorizationType, request.LocationId);
                await _unitOfWork.Resources.UpdateAsync(updateResource);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Recurso actualizado exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, $"Error de argumento al actualizar recurso con Id '{request.Id}': {argEx.Message}");
                return OperationResult.Failure(
                    argEx.Message,
                    "InvalidInput",
                    $"La operación de actualización falló debido a un error de argumento para el recurso con Id '{request.Id}'."
                );
            } catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, $"Fallo al actualizar recurso con Id '{request.Id}' debido a un error de persistencia.");
                return OperationResult.Failure(
                    pEx.Message,
                    "PersistenceError",
                    $"La operación de actualización falló debido a un problema de almacenamiento de datos para el recurso con Id '{request.Id}'."
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar recurso con Id '{ResourceId}': {ErrorMessage}", request.Id, ex.Message);
                return OperationResult.Failure(
                    "Ocurrió un error inesperado al actualizar el recurso.",
                    "UnexpectedError",
                    $"La operación de actualización falló debido a un error inesperado para el recurso con Id '{request.Id}'."
                );
            }
        }
    }
}
