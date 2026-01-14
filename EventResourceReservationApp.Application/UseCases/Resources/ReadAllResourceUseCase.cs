using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Domain.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Resources
{
    public class ReadAllResourceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadAllResourceUseCase> _logger;
        public ReadAllResourceUseCase(IUnitOfWork unitOfWork, ILogger<ReadAllResourceUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<ResourceResponse>>> ExecuteAsync(ReadAllResourceRequest request)
        {
            try
            {
                var resources = await _unitOfWork.Resources.GetAllAsync(
                    filter: r =>
                        (string.IsNullOrEmpty(request.Name) || r.Name.ToLower().Contains(request.Name.ToLower())) &&
                        (request.CategoryId == null || r.CategoryId == request.CategoryId) &&
                        (request.StatusId == null || r.StatusId == request.StatusId) &&
                        (request.CreatedByUserIdFilter == null || r.CreatedByUserId == request.CreatedByUserIdFilter) &&
                        (request.MinPrice == null || r.Price >= request.MinPrice) &&
                        (request.MaxPrice == null || r.Price <= request.MinPrice),
                    orderBy: q =>
                    {
                        return request.OrderBy?.ToLower() switch
                        {
                            "name_asc" => q.OrderBy(r => r.Name),
                            "name_desc" => q.OrderByDescending(r => r.Name),
                            "createdAt_asc" => q.OrderBy(r => r.CreatedAt),
                            "createdAt_desc" => q.OrderByDescending(r => r.CreatedAt),
                            _ => q.OrderByDescending(r => r.CreatedAt),
                        };
                    }
                );
                var resourceResponses = resources.Select(r => new ResourceResponse
                {
                    Id = r.Id,
                    StatusId = (int)r.StatusId,
                    StatusName = r.StatusId.ToString(),
                    StatusDescription =  r.StatusId.GetDescription(),
                    Name = r.Name,
                    Description = r.Description,
                    Price = r.Price,
                    Quantity = r.Quantity,
                    QuantityInUse = 0,
                    CategoryId = r.CategoryId,
                    CategoryName = r.Category?.Name ?? "",
                    LocationId = r.LocationId,
                    LocationDescription = r.Location?.City ?? "",
                    AuthorizationType = (int)r.AuthorizationType,
                    AuthorizationTypeName = r.AuthorizationType.ToString(),
                    AuthorizationTypeDescription = r.AuthorizationType.GetDescription(),
                    Created = r.CreatedAt
                });
                return OperationResult<IEnumerable<ResourceResponse>>.Success(resourceResponses, "Recursos obtenidos exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Error de persistencia al obtener todos los recursos.");
                return OperationResult<IEnumerable<ResourceResponse>>.Failure(
                    "Error al obtener los recursos.",
                    "PersistenceError",
                    "Ocurrió un error de persistencia al intentar obtener los recursos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurrió un error inesperado durante la obtención de los recursos.");
                return OperationResult<IEnumerable<ResourceResponse>>.Failure(
                    "Ocurrió un error interno imprevisto.",
                "UnexpectedError",
                    $"La operación de lectura falló debido a un problema inesperado para los recursos."
                );
            }
        }
    }
}
