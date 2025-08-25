using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Locations;
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
    public class ReadByIdLocationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadByIdLocationUseCase> _logger;
        public ReadByIdLocationUseCase(IUnitOfWork unitOfWork, ILogger<ReadByIdLocationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<LocationResponse>> ExecuteAsync(int locationId)
        {
            try
            {
                Location location = await _unitOfWork.Locations.GetByIdAsync(locationId);
                if (location == null)
                {
                    _logger.LogWarning($"No se encontró una ubicación con el ID '{locationId}'.");
                    return OperationResult<LocationResponse>.Failure(
                        "No se encontró la ubicación solicitada.",
                        "NotFound",
                        $"No se encontró una ubicación con el ID '{locationId}'."
                    );
                }
                var response = new LocationResponse
                {
                    Id = location.Id,
                    Country = location.Country,
                    City = location.City,
                    ZipCode = location.ZipCode,
                    Street = location.Street,
                    Neighborhood = location.Neighborhood,
                    ExteriorNumber = location.ExteriorNumber,
                    InteriorNumber = location.InteriorNumber,
                    CreatedByUserId = location.CreatedByUserId,
                    CreatedAt = location.CreatedAt
                };
                return OperationResult<LocationResponse>.Success(response, "Ubicación encontrada exitosamente.");
            }
            catch(PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al obtener la ubicación debido a un error de persistencia.");
                return OperationResult<LocationResponse>.Failure(
                    "No se pudo obtener la ubicación de la base de datos.",
                    "PersistenceError",
                    "La operación de lectura falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la obtención de la ubicación en el caso de uso.");
                return OperationResult<LocationResponse>.Failure(
                    "Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de lectura falló debido a un problema inesperado."
                );
            }
        }
    }
}
