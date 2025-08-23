using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
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
    public class CreateLocationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateLocationUseCase> _logger;
        public CreateLocationUseCase(IUnitOfWork unitOfWork, ILogger<CreateLocationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;   
        }
        public async Task<OperationResult<LocationResponse>> ExecuteAsync(CreateLocationRequest request)
        {
            try
            {
                Location location = new Location(request.Country,request.City,request.ZipCode, request.Street, request.Neighborhood,
                    request.ExteriorNumber, request.InteriorNumber, request.CreatedByUserId);
                await _unitOfWork.Locations.AddAsync(location);
                await _unitOfWork.SaveAsync();
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
                    CreateAt = location.CreatedAt
                };
                return OperationResult<LocationResponse>.Success(response, "Ubicaion creada exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al crear ubicación debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult<LocationResponse>.Failure(
                    "La operación de creación falló debido a una entrada inválida.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al crear ubicación debido a un error de persistencia.");
                return OperationResult<LocationResponse>.Failure("No se pudo guardar la ubicación en la base de datos.",
                    "PersistenceError",
                    "La operación de creación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception exc)
            {
                _logger.LogError(500, exc, "Ocurrió un error inesperado durante la creación de la ubicación en el caso de uso.");
                return OperationResult<LocationResponse>.Failure("Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de creación falló debido a un problema inesperado."
                );
            }

        }
    }
}
