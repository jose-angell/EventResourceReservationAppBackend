using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Resources
{
    public class DeleteResourceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteResourceUseCase> _logger;
        public DeleteResourceUseCase(IUnitOfWork unitOfWork, ILogger<DeleteResourceUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(Guid resourceId)
        {
            var deleteResource = await _unitOfWork.Resources.GetByIdAsync(resourceId);
            if (deleteResource == null)
            {
                _logger.LogWarning($"Fallo al eliminar recurso: No se encontro el recurso con Id '{resourceId}'");
                return OperationResult.Failure(
                    $"No se encontro el recurso con Id '{resourceId}'",
                    "NotFound",
                    $"La operación de eliminación falló debido a que no se encontró el recurso con Id '{resourceId}'."
                );
            }
            try
            {
                //TODO: Verificar si el recurso está asociado a reservas antes de eliminarlo.

                //DateTime dateTimeStart = DateTime.Now;
                //DateTime dateTimeEnd = DateTime.;
                //var validarResource = await _unitOfWork.Resources.GetAllAsync(filter: r => (r.StatusId == 1 && r.Id == resourceId), dateTimeStart, dateTimeStart);
                //if(validarResource != null && validarResource.Quantity > 0)
                //{
                //    _logger.LogWarning($"Fallo al eliminar recurso: El recurso con Id '{resourceId}' se encuentra en uso.");
                //    return OperationResult.Failure(
                //       $"El recurso con Id '{resourceId}' se encuentra en uso.",
                //      "NotFound",
                //      $"La operación de eliminación falló debido a que el recurso con Id '{resourceId}' se encuentra en uso."
                //     );
                //}
                await _unitOfWork.Resources.RemoveAsync(deleteResource);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Recurso eliminado exitosamente.");  
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, $"Fallo al eliminar recurso con id '{resourceId}' debido a un error de persistencia.");
                return OperationResult.Failure(
                    "No se pudo eliminar el recurso de la base de datos.",
                    "PersistenceError",
                    $"La operación de eliminación falló debido a un problema de almacenamiento de datos para el recurso con Id '{resourceId}'."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurrió un error inesperado durante la eliminación del recurso con Id '{resourceId}' en el caso de uso.");
                return OperationResult.Failure(
                    "Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    $"La operación de eliminación falló debido a un problema inesperado para el recurso con Id '{resourceId}'."
                );
            }
        }
    }
}
