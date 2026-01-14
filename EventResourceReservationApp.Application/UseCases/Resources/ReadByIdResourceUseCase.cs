using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Resources
{
    public class ReadByIdResourceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadByIdResourceUseCase> _logger;
        public ReadByIdResourceUseCase(IUnitOfWork unitOfWork, ILogger<ReadByIdResourceUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ResourceResponse>> ExecuteAsync(Guid resourceId)
        {
            try
            {
                var resource = await _unitOfWork.Resources.GetByIdAsync(resourceId);
                if (resource == null)
                {
                    _logger.LogWarning($"Recurso con Id '{resourceId}' no encontrado.");
                    return OperationResult<ResourceResponse>.Failure(
                     $"No se enconto el recurso con Id '{resourceId}'",
                    "NotFound",
                    $"La operación de busqueda falló debido a que no se encontró el recurso con Id '{resourceId}'."
                    );
                }
                // TODO: el calculo de totales en uso solo se hace en GetByIdAndDateRangeAsync, revisar si es necesario implementarlo aqui tambien


                var response = new ResourceResponse
                {
                    Id = resource.Id,
                    StatusId = (int)resource.StatusId,
                    StatusName = resource.StatusId.ToString(),
                    StatusDescription = resource.StatusId.GetDescription(),
                    Name = resource.Name,
                    Description = resource.Description,
                    Price = resource.Price,
                    Quantity = resource.Quantity,
                    QuantityInUse = resource.Quantity, //resource.QuantityInUse,
                    CategoryId = resource.CategoryId,
                    CategoryName = resource.Category?.Name ?? "",
                    LocationId = resource.LocationId,
                    LocationDescription = resource.Location?.City ?? "",
                    AuthorizationType = (int)resource.AuthorizationType,
                    AuthorizationTypeName = resource.AuthorizationType.ToString(),
                    AuthorizationTypeDescription = resource.AuthorizationType.GetDescription(),
                    Created = resource.CreatedAt
                };
                return OperationResult<ResourceResponse>.Success(response, "Recurso encontrado exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, $"Fallo al obtener recurso con id '{resourceId}' debido a un error de persistencia.");
                return OperationResult<ResourceResponse>.Failure(
                    "No se pudo obtener el recurso de la base de datos.",
                    "PersistenceError",
                    $"La operación de lectura falló debido a un problema de almacenamiento de datos para el recurso con Id '{resourceId}'."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurrió un error inesperado durante la obtención del recurso con Id '{resourceId}' en el caso de uso.");
                return OperationResult<ResourceResponse>.Failure(
                    "Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    $"La operación de lectura falló debido a un problema inesperado para el recurso con Id '{resourceId}'."
                );
            }
        }
    }
}
