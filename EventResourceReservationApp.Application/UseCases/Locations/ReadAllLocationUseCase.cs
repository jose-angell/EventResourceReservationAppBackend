using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Loctions;
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
                Expression<Func<Location, bool>>? filter = null;
                Func<IQueryable<Location>, IOrderedQueryable<Location>>? orderBy = null;
                if (!string.IsNullOrEmpty(request.City))
                {
                    filter = c => c.City.Contains(request.City);
                }
                if (request.ZipCode > 0)
                {
                    if (filter != null)
                    {
                        Expression<Func<Location, bool>> newFilter = c => c.ZipCode == request.ZipCode;
                        var parameter = filter.Parameters.Single();
                        var body = Expression.AndAlso(filter.Body, Expression.Invoke(newFilter, parameter));
                        filter = Expression.Lambda<Func<Location, bool>>(body, parameter);
                    }
                    else
                    {
                        filter = c => c.ZipCode == request.ZipCode;
                    }
                }
                if (request.CreatedByUserIdFilter.HasValue && request.CreatedByUserIdFilter != Guid.Empty)
                {
                    if (filter != null)
                    {
                        Expression<Func<Location, bool>> newFilter = c => c.CreatedByUserId == request.CreatedByUserIdFilter;
                        var parameter = filter.Parameters.Single();
                        var body = Expression.AndAlso(filter.Body, Expression.Invoke(newFilter, parameter));
                        filter = Expression.Lambda<Func<Location, bool>>(body, parameter);
                    }
                    else
                    {
                        filter = c => c.CreatedByUserId == request.CreatedByUserIdFilter;
                    }
                }
                if (!string.IsNullOrEmpty(request.OrderBy))
                {
                    if (request.OrderBy.Equals("City_asc", StringComparison.OrdinalIgnoreCase))
                    {
                        orderBy = q => q.OrderBy(c => c.City);
                    }
                    else if (request.OrderBy.Equals("CreatedAt_asc", StringComparison.OrdinalIgnoreCase))
                    {
                        orderBy = q => q.OrderBy(c => c.CreatedAt);
                    }
                }
                var locations = await _unitOfWork.Locations.GetAllAsync(filter, orderBy);
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
                    CreateAt = l.CreatedAt
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
