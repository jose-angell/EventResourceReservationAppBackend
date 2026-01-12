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
    public class ReadByIdAndDateRangeResourceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadByIdAndDateRangeResourceUseCase> _logger;
        public ReadByIdAndDateRangeResourceUseCase(IUnitOfWork unitOfWork, ILogger<ReadByIdAndDateRangeResourceUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ResourceResponse>> ExecuteAsync(ReadByIdAndDateRangeResourceRequest request)
        {
            try
            {
                var resource = await _unitOfWork.Resources.GetByIdAsync(filter: r => (r.Id == request.Id), request.StartTime,request.EndTime);
                if (resource == null)
                {
                    _logger.LogWarning($"Recurso con Id '{request.Id}' no encontrado.");
                    return OperationResult<ResourceResponse>.Failure(
                     $"No se enconto el recurso con Id '{request.Id}'",
                    "NotFound",
                    $"La operación de busqueda falló debido a que no se encontró el recurso con Id '{request.Id}'."
                    );
                }
                // TODO: Calcular la cantidad en uso entre las fechas proporcionadas


                
                return OperationResult<ResourceResponse>.Success(resource, "Recurso encontrado exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, $"Fallo al obtener recurso con id '{request.Id}' debido a un error de persistencia.");
                return OperationResult<ResourceResponse>.Failure(
                    "No se pudo obtener el recurso de la base de datos.",
                "PersistenceError",
                    $"La operación de lectura falló debido a un problema de almacenamiento de datos para el recurso con Id '{request.Id}'."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurrió un error inesperado durante la obtención del recurso con Id '{request.Id}' en el caso de uso.");
                return OperationResult<ResourceResponse>.Failure(
                    "Ocurrió un error interno imprevisto.",
                "UnexpectedError",
                    $"La operación de lectura falló debido a un problema inesperado para el recurso con Id '{request.Id}'."
                );
            }
        }
    }
}
