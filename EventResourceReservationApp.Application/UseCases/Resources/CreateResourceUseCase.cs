using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.UseCases.Resources
{
    public class CreateResourceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateResourceUseCase> _logger;
        public CreateResourceUseCase(IUnitOfWork unitOfWork, ILogger<CreateResourceUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<ResourceResponse>> ExecuteAsync(CreateResourceRequest request)
        {
            var existingResource = await _unitOfWork.Resources
                .GetFirstOrDefaultAsync(r => r.Name.ToLower() == request.Name.ToLower() && r.LocationId == request.LocationId);
            if (existingResource != null)
            {
                _logger.LogWarning($"Fallo al crear recurso: Ya existe un recurso con el nombre '{request.Name}' en la ubicación '{request.LocationId}'.");
                return OperationResult<ResourceResponse>.Failure(
                    $"Ya existe un recurso con el nombre '{request.Name}' en la ubicación especificada.",
                    "Conflict",
                    $"La operación de creación falló debido a una duplicación de nombre '{request.Name}' en la ubicación '{request.LocationId}'."
                );
            }
            try
            {
                Resource resource = new Resource(
                    request.CategoryId,request.Name, request.Description, request.Quantity, request.Price,
                    request.AuthorizationType, request.LocationId, request.CreatedByUserId
                );
                await _unitOfWork.Resources.AddAsync(resource);
                await _unitOfWork.SaveAsync();
                var response = new ResourceResponse
                {
                    Id = resource.Id,
                    StatusId = resource.StatusId,
                    StatusDescription = "",
                    Name = resource.Name,
                    Description = resource.Description,
                    Price = resource.Price,
                    Quantity = resource.Quantity,
                    QuantityInUse = resource.Quantity,
                    CategoryId = resource.CategoryId,
                    CategoryName = "",
                    LocationId = resource.LocationId,
                    LocationDescription = "",
                    AuthorizationType = resource.AuthorizationType,
                    Created = resource.CreatedAt
                };
                return OperationResult<ResourceResponse>.Success(response, "Recurso creado exitosamente.");
            }
            catch(ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Fallo al crear el recurso debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult<ResourceResponse>.Failure(
                    "La operación de creación falló debido a una entrada inválida.",
                    "InvalidInput",
                    argEx.Message
                );
            }catch(PersistenceException pEx)
            {
                _logger.LogError(500, pEx, "Fallo al crear el recurso debido a un error de persistencia.");
                return OperationResult<ResourceResponse>.Failure(
                    "No se pudo guardar el recurso en la base de datos.",
                    "PersistenceError",
                    "La operación de creación falló debido a un problema de almacenamiento de datos."
                );
            }catch(Exception ex)
            {
                _logger.LogError(500, ex, "Fallo inesperado al crear el recurso.");
                return OperationResult<ResourceResponse>.Failure(
                    "Ocurrió un error inesperado durante la creación del recurso.",
                    "UnexpectedError",
                    "La operación de creación falló debido a un error inesperado."
                );
            }
        }
    }
}
