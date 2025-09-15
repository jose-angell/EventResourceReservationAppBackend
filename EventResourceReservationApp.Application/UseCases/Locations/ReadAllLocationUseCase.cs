using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Locations;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Locations
{
    public class ReadAllLocationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadAllLocationUseCase> _logger;
        public ReadAllLocationUseCase(IUnitOfWork unitOfWork, ILogger<ReadAllLocationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<LocationResponse>>> ExecuteAsync(ReadAllLocationRequest request)
        {
            try
            {
                var locations = await _unitOfWork.Locations.GetAllAsync(
                    filter : l =>
                        (string.IsNullOrEmpty(request.City) || l.City.Contains(request.City)) &&
                        (request.ZipCode == null || l.ZipCode == request.ZipCode) &&
                        (request.CreatedByUserIdFilter == null || l.CreatedByUserId == request.CreatedByUserIdFilter),
                    orderBy: q =>
                    {
                        return request.OrderBy?.ToLower() switch
                        {
                            "City_asc" => q.OrderBy(l => l.City),
                            "CreatedAt_asc" => q.OrderBy(l => l.CreatedAt),
                            _ => q.OrderByDescending(l => l.CreatedAt),
                        };
                    }
                );
                var locationResponses = locations.Select(l => new LocationResponse
                {
                    Id = l.Id,
                    Country = l.Country,
                    City = l.City,
                    ZipCode = l.ZipCode,
                    Street = l.Street,
                    Neighborhood = l.Neighborhood,
                    ExteriorNumber = l.ExteriorNumber,
                    InteriorNumber = l.InteriorNumber,
                    CreatedByUserId = l.CreatedByUserId,
                    CreatedAt = l.CreatedAt
                });
                return OperationResult<IEnumerable<LocationResponse>>.Success(locationResponses, "Ubicaciones obtenidas exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al obtener las ubicacines debido a un error de persistencia.");
                return OperationResult<IEnumerable<LocationResponse>>.Failure(
                    "No se pudieron obtener las ubicación de la base de datos.",
                    "PersistenceError",
                    "La operación de lectura falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la obtención de las ubicaciones en el caso de uso.");
                return OperationResult<IEnumerable<LocationResponse>>.Failure(
                    "Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de lectura falló debido a un problema inesperado."
                );
            }
        }
    }
}
