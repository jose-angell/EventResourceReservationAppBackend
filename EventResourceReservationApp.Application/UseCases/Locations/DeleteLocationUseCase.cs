using EventResourceReservationApp.Application.Common;
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
    public class DeleteLocationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteLocationUseCase> _logger;
        public DeleteLocationUseCase(IUnitOfWork unitOfWork, ILogger<DeleteLocationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(int LocationId)
        {
            Location deleteLocation = await _unitOfWork.Locations.GetByIdAsync(LocationId);
            if(deleteLocation == null)
            {
                _logger.LogWarning($"Fallo al eliminar: No se encontró una ubicación con el ID '{LocationId}'.");
                return OperationResult.Failure($"No se encontró una ubicación con el ID '{LocationId}'.",
                        "NotFound",
                        "La operación de actualización falló porque la ubicación no existe."
                );
            }
            try
            {
                await _unitOfWork.Locations.RemoveAsync(deleteLocation);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Ubicación eliminada exitosamente.");
            }
            catch(PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al eliminar la ubicación debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo eliminar la ubicación de la base de datos.",
                    "PersistenceError",
                    "La operación de eliminación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la eliminación de la ubicación en el caso de uso.");
                return OperationResult.Failure("Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de eliminación falló debido a un problema inesperado."
                );
            }
        }
    }
}
