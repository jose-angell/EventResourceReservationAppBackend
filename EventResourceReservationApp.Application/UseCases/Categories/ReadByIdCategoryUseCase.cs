using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace EventResourceReservationApp.Application.UseCases.Categories
{
    public class ReadByIdCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReadByIdCategoryUseCase> _logger;
        public ReadByIdCategoryUseCase(IUnitOfWork unitOfWork, ILogger<ReadByIdCategoryUseCase> logger )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<CategoryResponse>> ExecuteAsync(int id)
        {
            try
            {
                var caategory = await _unitOfWork.Categories.GetByIdAsync(id);
                if (caategory == null)
                {
                    _logger.LogWarning("Fallo al consultar: No se encontró una categoría con el ID '{CategoryId}'.", id);
                    return OperationResult<CategoryResponse>.Failure(
                            $"No se encontró una categoría con el ID '{id}'.",
                            "NotFound",
                            "La operación de consulta falló porque la categoría no existe."
                    );
                }
                var categoryDto = new CategoryResponse
                {
                    Id = caategory.Id,
                    Name = caategory.Name,
                    Description = caategory.Description,
                    CreatedAt = caategory.CreatedAt,
                    CreatedByUserId = caategory.CreatedByUserId
                };
                return OperationResult<CategoryResponse>.Success(categoryDto, "Categoria encontrada exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al obtener la categoría {CategoryId} debido a un error de persistencia.", id);
                return OperationResult<CategoryResponse>.Failure(
                    "No se pudo obtener la categoría de la base de datos.",
                    "PersistenceError",
                    "La operación de consulta falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la obtención de la categoría {CategoryId}.", id);
                return OperationResult<CategoryResponse>.Failure(
                    "Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de consulta falló debido a un problema inesperado."
                );
            }
        }
    }
}
